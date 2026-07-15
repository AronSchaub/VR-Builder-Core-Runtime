// Modifications copyright (c) 2026 Aron Schaub
// SPDX-License-Identifier: Apache-2.0

using Newtonsoft.Json;
using System;
using System.Collections;
using System.Runtime.Serialization;
using VRBuilder.Core.Attributes;
using VRBuilder.Core.Properties;
using VRBuilder.Core.SceneObjects;
using VRBuilder.Core.Utils;

namespace VRBuilder.Core.Behaviors
{
    /// <summary>
    /// This behavior changes the parent of a game object in the scene hierarchy. It can accept a null parent, in which case the object will be unparented.
    /// </summary>
    [DataContract(IsReference = true)]
    [HelpLink("https://mindport-gmbh.github.io/VR-Builder-Documentation/articles/core/set-parent-behavior.html?utm_source=unity_editor&utm_medium=referral&utm_campaign=from_unity&utm_id=from_unity")]
    public class SetParentBehavior : Behavior<SetParentBehavior.EntityData>
    {
        [DisplayName("Set Parent")]
        [DataContract(IsReference = true)]
        public class EntityData : IBehaviorData
        {
            /// <summary>
            /// Process object to reparent.
            /// </summary>
            [DataMember]
            [DisplayName("Target Object")]
            [DisplayTooltip("Process object to reparent.")]
            public SingleScenePropertyReference<IModifyParentProperty> TargetObject { get; set; }

            /// <summary>
            /// New parent game object.
            /// </summary>
            [DataMember]
            [DisplayName("Parent Object")]
            [DisplayTooltip("New parent game object. Leave empty to unparent the target object.")]
            public SingleSceneObjectReference ParentObject { get; set; }

            /// <summary>
            /// If true, the object will be moved to the parent's transform.
            /// </summary>
            [DataMember]
            [DisplayName("Snap to parent transform")]
            public bool SnapToParentTransform { get; set; }

            public Metadata Metadata { get; set; }

            [IgnoreDataMember]
            public string Name => ParentObject.HasValue() ? $"Make {TargetObject} child of {ParentObject}" : $"Unparent {TargetObject}";
        }

        [JsonConstructor]
        public SetParentBehavior() : this(Guid.Empty, Guid.Empty)
        {
        }

        public SetParentBehavior(ISceneObject target, ISceneObject parent, bool snapToParentTransform = false) : this(ProcessReferenceUtils.GetUniqueIdFrom(target), ProcessReferenceUtils.GetUniqueIdFrom(parent), snapToParentTransform)
        {
        }

        public SetParentBehavior(Guid target, Guid parent, bool snapToParentTransform = false)
        {
            Data.TargetObject = new SingleScenePropertyReference<IModifyParentProperty>(target);
            Data.ParentObject = new SingleSceneObjectReference(parent);
            Data.SnapToParentTransform = snapToParentTransform;
        }

        private class ActivatingProcess : StageProcess<EntityData>
        {
            public ActivatingProcess(EntityData data) : base(data)
            {
            }

            /// <inheritdoc />
            public override void Start()
            {
                if (Data.ParentObject.HasValue())
                    Data.TargetObject.Value.SetParent(Data.ParentObject.Value, Data.SnapToParentTransform);
                else
                    Data.TargetObject.Value.UnsetParent();
            }

            /// <inheritdoc />
            public override IEnumerator Update()
            {
                yield return null;
            }

            /// <inheritdoc />
            public override void End()
            {
            }

            /// <inheritdoc />
            public override void FastForward()
            {
            }
        }

        /// <inheritdoc />
        public override IStageProcess GetActivatingProcess()
        {
            return new ActivatingProcess(Data);
        }
    }
}