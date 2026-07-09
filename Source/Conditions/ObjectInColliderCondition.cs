// Modifications copyright (c) 2026 Aron Schaub
// SPDX-License-Identifier: Apache-2.0

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using VRBuilder.Core.Attributes;
using VRBuilder.Core.Properties;
using VRBuilder.Core.SceneObjects;
using VRBuilder.Core.Settings;
using VRBuilder.Core.Utils;

namespace VRBuilder.Core.Conditions
{
    /// <summary>
    /// Condition which is completed when `TargetObject` gets inside `TriggerProperty`'s collider.
    /// </summary>
    [DataContract(IsReference = true)]
    [HelpLink("https://www.mindport.co/vr-builder/manual/default-conditions/move-object-in-collider")]
    public class ObjectInColliderCondition : Condition<ObjectInColliderCondition.EntityData>
    {
        /// <summary>
        /// The "object in collider" condition's data.
        /// </summary>
        [DisplayName("Move Object into Collider")]
        [DataContract(IsReference = true)]
        public class EntityData : IObjectInTargetData
        {
            /// <summary>
            /// The objects that have to enter the collider.
            /// </summary>
            [DataMember]
            [DisplayName("Objects")]
            public MultipleSceneObjectReference TargetObjects { get; set; }

            /// <summary>
            /// The collider with trigger to enter.
            /// </summary>
            [DataMember]
            [DisplayName("Collider")]

            public SingleScenePropertyReference<IColliderWithTriggerProperty> TriggerObject { get; set; }

            /// <inheritdoc />
            public bool IsCompleted { get; set; }

            /// <inheritdoc />
            [HideInProcessInspector]
            [IgnoreDataMember]
            public string Name
            {
                get
                {
                    if (ObjectsRequiredInTrigger > 1 && TargetObjects.Values.Count() > 1)
                    {
                        return $"Move {ObjectsRequiredInTrigger} of {TargetObjects.Values.Count()} '{SceneObjectGroups.Instance.GetLabel(TargetObjects.Guids.First())}' in collider {TriggerObject}";
                    }

                    return $"Move {TargetObjects} in collider {TriggerObject}";
                }
            }

            /// <inheritdoc />
            [DataMember]
            [DisplayName("Required seconds inside")]
            public float RequiredTimeInside { get; set; }

            [DataMember]
            [DisplayName("Required Object count")]
            public float ObjectsRequiredInTrigger { get; set; }

            /// <inheritdoc />
            public Metadata Metadata { get; set; }
        }

        [JsonConstructor]
        public ObjectInColliderCondition() : this(Guid.Empty, Guid.Empty)
        {
        }

        public ObjectInColliderCondition(IColliderWithTriggerProperty targetPosition, IReadOnlyList<Guid> multipleObjectsGuids, int requiredTimeInTarget, int objectsRequiredInTrigger)
            : this(ProcessReferenceUtils.GetUniqueIdFrom(targetPosition), multipleObjectsGuids, requiredTimeInTarget, objectsRequiredInTrigger)
        {
        }

        // ReSharper disable once SuggestBaseTypeForParameter
        public ObjectInColliderCondition(IColliderWithTriggerProperty targetPosition, ISceneObject targetObject, float requiredTimeInTarget = 0, float objectsRequiredInTrigger = 1)
            : this(ProcessReferenceUtils.GetUniqueIdFrom(targetPosition), ProcessReferenceUtils.GetUniqueIdFrom(targetObject), requiredTimeInTarget, objectsRequiredInTrigger)
        {
        }

        public ObjectInColliderCondition(Guid targetPosition, Guid targetObject, float requiredTimeInTarget = 0, float objectsRequiredInTrigger = 1)
        {
            Data.TriggerObject = new SingleScenePropertyReference<IColliderWithTriggerProperty>(targetPosition);
            Data.TargetObjects = new MultipleSceneObjectReference(targetObject);
            Data.RequiredTimeInside = requiredTimeInTarget;
            Data.ObjectsRequiredInTrigger = objectsRequiredInTrigger;
        }

        private ObjectInColliderCondition(Guid targetPosition, IReadOnlyList<Guid> targetObject, int requiredTimeInTarget, int objectsRequiredInTrigger)
        {
            Data.TriggerObject = new SingleScenePropertyReference<IColliderWithTriggerProperty>(targetPosition);
            Data.TargetObjects = new MultipleSceneObjectReference(targetObject);
            Data.RequiredTimeInside = requiredTimeInTarget;
            Data.ObjectsRequiredInTrigger = objectsRequiredInTrigger;
        }

        private class ActiveProcess : ObjectInTargetActiveProcess<EntityData>
        {
            public ActiveProcess(EntityData data) : base(data)
            {
            }

            public override void Start()
            {
                if (Data.ObjectsRequiredInTrigger > Data.TargetObjects.Values.Count())
                {
                    ForwardingLogger.LogWarning($"The required object count {Data.ObjectsRequiredInTrigger} is bigger then the target objects count of {Data.TargetObjects.Values.Count()} and not completable.");
                }
                if (Data.ObjectsRequiredInTrigger < 1)
                {
                    ForwardingLogger.LogWarning($"The required object count {Data.ObjectsRequiredInTrigger} is below 1 and always completed.");
                }
                base.Start();
            }

            /// <inheritdoc />
            protected override bool IsInside()
            {
                int counter = 0;

                foreach (ISceneObject sceneObject in Data.TargetObjects.Values)
                {
                    counter += Data.TriggerObject.Value.IsTransformInsideTrigger(sceneObject) ? 1 : 0;
                }

                return counter >= Data.ObjectsRequiredInTrigger;
            }
        }

        private class EntityAutocompleter : Autocompleter<EntityData>
        {
            public EntityAutocompleter(EntityData data) : base(data)
            {
            }

            /// <inheritdoc />
            public override void Complete()
            {
                if (Data.ObjectsRequiredInTrigger > 1 && Data.TargetObjects.Values.Any())
                {
                    int counter = 0;
                    foreach (var objs in Data.TargetObjects.Values)
                    {
                        Data.TriggerObject.Value.FastForwardEnter(objs);
                        counter++;
                        if (counter >= Data.ObjectsRequiredInTrigger)
                        {
                            break;
                        }
                    }
                }
                else
                {
                    ISceneObject sceneObject = Data.TargetObjects.Values.FirstOrDefault();
                    Data.TriggerObject.Value.FastForwardEnter(sceneObject);
                }
            }
        }

        /// <inheritdoc />
        public override IStageProcess GetActiveProcess()
        {
            return new ActiveProcess(Data);
        }

        /// <inheritdoc />
        protected override IAutocompleter GetAutocompleter()
        {
            return new EntityAutocompleter(Data);
        }
    }
}
