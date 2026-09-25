// Copyright (c) 2013-2019 Innoactive GmbH
// Licensed under the Apache License, Version 2.0
// Modifications copyright (c) 2021-2026 MindPort GmbH

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using VRBuilder.Core.Attributes;
using VRBuilder.Core.Configuration;
using VRBuilder.Core.Configuration.Modes;
using VRBuilder.Core.EntityOwners;
using VRBuilder.Core.EntityOwners.FoldedEntityCollection;
using VRBuilder.Core.EntityOwners.ParallelEntityCollection;
using VRBuilder.Core.Primitives;
using VRBuilder.Core.Properties;
using VRBuilder.Core.RestrictiveEnvironment;
using VRBuilder.Core.Runtime.Registry;
using VRBuilder.Core.SceneObjects;
using VRBuilder.Core.StepLocking;
using VRBuilder.Core.Utils.Logging;
using VRBuilder.Core.Utils;
using VRBuilder.Utils;

namespace VRBuilder.Core
{
    /// <summary>
    /// An implementation of <see cref="IStep"/> interface.
    /// </summary>
    [DataContract(IsReference = true)]
    public class Step : Entity<Step.EntityData>, IStep
    {
        public class EntityData : EntityCollectionData<IStepChild>, IStepData, ILockableStepData
        {
            ///<inheritdoc />
            [DataMember]
            [DrawingPriority(0)]
            [HideInProcessInspector]
            public string Name { get; set; }

            ///<inheritdoc />
            [DataMember]
            [DrawingPriority(1)]
            [UsesSpecificProcessDrawer("MultiLineStringDrawer")]
            public string Description { get; set; }

            ///<inheritdoc />
            [DataMember]
            [HideInProcessInspector]
            public IBehaviorCollection Behaviors { get; set; }

            ///<inheritdoc />
            [DataMember]
            [HideInProcessInspector]
            public ITransitionCollection Transitions { get; set; }

            ///<inheritdoc />
            public override IEnumerable<IStepChild> GetChildren()
            {
                return new List<IStepChild>
                {
                    Behaviors,
                    Transitions
                };
            }

            /// <inheritdoc />
            public void SetName(string name)
            {
                Name = name;
            }

            ///<inheritdoc />
            [IgnoreDataMember]
            public IStepChild Current { get; set; }

            ///<inheritdoc />
            public IMode Mode { get; set; }

            ///<inheritdoc />
            [DataMember]
            [HideInProcessInspector]
            public IEnumerable<LockablePropertyReference> ToUnlock { get; set; } = new List<LockablePropertyReference>();

            [DataMember]
            [HideInProcessInspector]
            public IDictionary<Guid, IEnumerable<Type>> GroupsToUnlock { get; set; } = new Dictionary<Guid, IEnumerable<Type>>();

            /// <inheritdoc />
            IEntity IEntitySequenceData.Current => Current;

            public EntityData()
            {
            }
        }

        public override void Configure(IMode mode)
        {
            try
            {
                base.Configure(mode);
            }
            catch (Exception e)
            {
                string fullPath = EntityPathUtils.BuildRichTextEntityPath(this);
                ForwardingLogger.LogError($"Configure failed at {fullPath}\nException: {e.Message}");
                ForwardingLogger.LogException(e);
            }
        }

        private class UnlockProcess : StageProcess<EntityData>
        {
            private readonly IEnumerable<LockablePropertyData> toUnlock;

            public UnlockProcess(EntityData data) : base(data)
            {
                toUnlock = Data.ToUnlock.Select(reference => new LockablePropertyData(reference.GetProperty())).ToList();

                foreach (Guid tag in Data.GroupsToUnlock.Keys)
                {
                    foreach (ISceneObject sceneObject in ServiceRegistry.Get<ISceneObjectRegistry>().GetObjects(tag))
                    {
                        toUnlock = toUnlock.Union(sceneObject.Properties.Where(property => Data.GroupsToUnlock[tag].Contains(property.GetType())).Select(property => new LockablePropertyData(property as LockableProperty))).ToList();
                    }
                }
            }

            ///<inheritdoc />
            public override void Start()
            {
                ServiceRegistry.Get<IStepLockService>()?.Unlock(Data, toUnlock);
            }

            ///<inheritdoc />
            public override IEnumerator Update()
            {
                yield return null;
            }

            ///<inheritdoc />
            public override void End()
            {
            }

            ///<inheritdoc />
            public override void FastForward()
            {
            }
        }

        private class LockProcess : StageProcess<EntityData>
        {
            private readonly IEnumerable<LockablePropertyData> toUnlock;

            public LockProcess(EntityData data) : base(data)
            {
                toUnlock = Data.ToUnlock.Select(reference => new LockablePropertyData(reference.GetProperty())).ToList();

                foreach (Guid tag in Data.GroupsToUnlock.Keys)
                {
                    foreach (ISceneObject sceneObject in ServiceRegistry.Get<ISceneObjectRegistry>().GetObjects(tag))
                    {
                        toUnlock = toUnlock.Union(sceneObject.Properties.Where(property => Data.GroupsToUnlock[tag].Contains(property.GetType())).Select(property => new LockablePropertyData(property as LockableProperty))).ToList();
                    }
                }
            }

            ///<inheritdoc />
            public override void Start()
            {
            }

            ///<inheritdoc />
            public override IEnumerator Update()
            {
                yield return null;
            }

            ///<inheritdoc />
            public override void End()
            {
                ServiceRegistry.Get<IStepLockService>()?.Lock(Data, toUnlock);
            }

            ///<inheritdoc />
            public override void FastForward()
            {
            }
        }

        private class ActiveProcess : StageProcess<EntityData>
        {
            private readonly IEnumerable<LockablePropertyData> toUnlock;

            public ActiveProcess(EntityData data) : base(data)
            {
            }

            ///<inheritdoc />
            public override void Start()
            {
            }

            ///<inheritdoc />
            public override IEnumerator Update()
            {
                while (HasCompletedTransition() == false)
                {
                    yield return null;
                }
            }

            private bool HasCompletedTransition()
            {
                IEntity[] transitions = RuntimeEntityGraph.GetChildren(Data.Transitions.Data);
                for (int i = 0; i < transitions.Length; i++)
                {
                    if (((ITransition)transitions[i]).IsCompleted)
                    {
                        return true;
                    }
                }

                return false;
            }

            ///<inheritdoc />
            public override void End()
            {
            }

            ///<inheritdoc />
            public override void FastForward()
            {
            }
        }

        private class AbortingProcess : InstantProcess<EntityData>
        {
            private readonly IEnumerable<LockablePropertyData> lockableProperties;

            public AbortingProcess(EntityData data) : base(data)
            {
                lockableProperties = Data.ToUnlock.Select(reference => new LockablePropertyData(reference.GetProperty())).ToList();

                foreach (Guid tag in Data.GroupsToUnlock.Keys)
                {
                    foreach (ISceneObject sceneObject in ServiceRegistry.Get<ISceneObjectRegistry>().GetObjects(tag))
                    {
                        lockableProperties = lockableProperties.Union(sceneObject.Properties.Where(property => Data.GroupsToUnlock[tag].Contains(property.GetType())).Select(property => new LockablePropertyData(property as LockableProperty))).ToList();
                    }
                }
            }

            public override void Start()
            {
                ServiceRegistry.Get<IStepLockService>()?.Lock(Data, lockableProperties);
            }
        }

        ///<inheritdoc />
        [DataMember]
        public StepMetadata StepMetadata { get; set; }

        /// <inheritdoc />
        public override void RegenerateId()
        {
            base.RegenerateId();

            if (StepMetadata != null)
            {
                StepMetadata.Guid = Id;
            }
        }

        ///<inheritdoc />
        public override IStageProcess GetActivatingProcess()
        {
            return new CompositeProcess(new FoldedActivatingProcess<IStepChild>(Data));
        }

        ///<inheritdoc />
        public override IStageProcess GetActiveProcess()
        {
            return new CompositeProcess(new FoldedActiveProcess<IStepChild>(Data), new ActiveProcess(Data), new UnlockProcess(Data));
        }

        ///<inheritdoc />
        public override IStageProcess GetDeactivatingProcess()
        {
            return new CompositeProcess(new FoldedDeactivatingProcess<IStepChild>(Data), new LockProcess(Data));
        }

        ///<inheritdoc />
        public override IStageProcess GetAbortingProcess()
        {
            return new CompositeProcess(new AbortingProcess(Data), new ParallelAbortingProcess<EntityData>(Data));
        }

        ///<inheritdoc />
        protected override IConfigurator GetConfigurator()
        {
            return new FoldedLifeCycleConfigurator<IStepChild>(Data);
        }

        ///<inheritdoc />
        IStepData IDataOwner<IStepData>.Data
        {
            get { return Data; }
        }

        protected Step() : this(null)
        {
        }

        public Step(string name)
        {
            StepMetadata = new StepMetadata();
            StepMetadata.Guid = Id;

            Data.Transitions = new TransitionCollection();
            Data.Behaviors = new BehaviorCollection();
            Data.Name = name;

            if (ServiceRegistry.Get<IRuntimeService>().LifeCycleLogging.LogSteps)
            {
                LifeCycle.StageChanged += (sender, args) => { ForwardingLogger.LogFormat("{0}<b>Step</b> <i>'{1}'</i> is <b>{2}</b>.\n", ConsoleUtils.GetTabs(), Data.Name, LifeCycle.Stage); };
            }
        }

        /// <summary>
        /// Creates a new <see cref="IStep"/>.
        /// </summary>
        /// <param name="name"><see cref="IStep"/>'s name.</param>
        /// <param name="position">The step's position in the process graph.</param>
        /// <param name="stepType">The step's type identifier.</param>
        /// <returns>The created <see cref="IStep"/>.</returns>
        public static IStep Create(string name, IVector2 position = default, string stepType = "default")
        {
            IStep step = new Step(name);
            step.StepMetadata.Position = position;
            step.StepMetadata.StepType = stepType;
            // PostProcessEntity<IStep>(step);

            return step;
        }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            StepMetadata = StepMetadata ?? new StepMetadata();

            if (StepMetadata.Guid == Guid.Empty)
            {
                StepMetadata.Guid = Id;
            }
            else
            {
                SetId(StepMetadata.Guid);
            }
        }
    }
}
