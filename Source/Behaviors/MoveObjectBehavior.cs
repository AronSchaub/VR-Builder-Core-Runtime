// Modifications copyright (c) 2026 Aron Schaub
// SPDX-License-Identifier: Apache-2.0

using System;
using System.Collections;
using System.Diagnostics;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using VRBuilder.Core.Attributes;
using VRBuilder.Core.Primitives;
using VRBuilder.Core.Properties;
using VRBuilder.Core.SceneObjects;
using VRBuilder.Core.Utils;

namespace VRBuilder.Core.Behaviors
{
    /// <summary>
    /// Behavior that moves target SceneObject to the position and rotation of another TargetObject.
    /// It takes `Duration` seconds, even if the target was in the place already.
    /// If `Duration` is equal or less than 0, transition is instantaneous.
    /// </summary>
    [DataContract(IsReference = true)]
    [HelpLink("https://mindport-gmbh.github.io/VR-Builder-Documentation/articles/core/move-object-behavior.html?utm_source=unity_editor&utm_medium=referral&utm_campaign=from_unity&utm_id=from_unity")]
    public class MoveObjectBehavior : Behavior<MoveObjectBehavior.EntityData>
    {
        /// <summary>
        /// The "move object" behavior's data.
        /// </summary>
        [DisplayName("Move Object")]
        [DataContract(IsReference = true)]
        public class EntityData : IBehaviorData
        {
            /// <summary>
            /// Target scene object to be moved.
            /// </summary>
            [DataMember]
            [DisplayName("Object")]
            public SingleScenePropertyReference<IMoveProperty> TargetObject { get; set; }

            /// <summary>
            /// Target's position and rotation is linearly interpolated to match PositionProvider's position and rotation at the end of transition.
            /// </summary>
            [DataMember]
            [DisplayName("Final position provider")]
            public SingleSceneObjectReference FinalPosition { get; set; }

            /// <summary>
            /// Duration of the transition. If duration is equal or less than zero, target object movement is instantaneous.
            /// </summary>
            [DataMember]
            [DisplayName("Animation")]
            [DisplayTooltip("Duration of the transition in seconds. If zero or less, movement is instantaneous.")]
            public float Duration { get; set; }

            [DataMember]
            [DisplayName("Animation curve")]
            public IAnimationCurve AnimationCurve { get; set; }

            /// <inheritdoc />
            public Metadata Metadata { get; set; }

            /// <inheritdoc />
            [IgnoreDataMember]
            public string Name => $"Move {TargetObject} to {FinalPosition}";
        }

        private class ActivatingProcess : StageProcess<EntityData>
        {
            private readonly Stopwatch stopWatch = new();

            public ActivatingProcess(EntityData data) : base(data)
            {
            }

            /// <inheritdoc />
            public override void Start()
            {
                stopWatch.Restart();
                Data.TargetObject.Value.DisablePhysics();
            }

            /// <inheritdoc />
            public override IEnumerator Update()
            {
                while (stopWatch.ElapsedMilliseconds < Data.Duration)
                {
                    float progress = stopWatch.ElapsedMilliseconds / Data.Duration;
                    Data.TargetObject.Value.MoveTo(Data.FinalPosition.Value, progress, Data.AnimationCurve);
                    yield return null;
                }
            }

            /// <inheritdoc />
            public override void End()
            {
                Data.TargetObject.Value.MoveTo(Data.FinalPosition.Value, 1f);
                Data.TargetObject.Value.EnablePhysics();
                stopWatch.Stop();
            }

            public override void FastForward()
            {
                Data.TargetObject.Value.DisablePhysics();
                Data.TargetObject.Value.MoveTo(Data.FinalPosition.Value, 1f);
                Data.TargetObject.Value.EnablePhysics();
            }
        }

        [JsonConstructor]
        public MoveObjectBehavior() : this(Guid.Empty, Guid.Empty, 0f)
        {
        }

        public MoveObjectBehavior(ISceneObject target, ISceneObject positionProvider, float duration) : this(ProcessReferenceUtils.GetUniqueIdFrom(target), ProcessReferenceUtils.GetUniqueIdFrom(positionProvider), duration)
        {
        }

        public MoveObjectBehavior(Guid targetObjectId, Guid finalPositionId, float duration)
        {
            Data.TargetObject = new SingleScenePropertyReference<IMoveProperty>(targetObjectId);
            Data.FinalPosition = new SingleSceneObjectReference(finalPositionId);
            Data.Duration = duration;
            Data.AnimationCurve = AnimationCurveData.Linear(0f, 0f, 1f, 1f);
        }

        /// <inheritdoc />
        public override IStageProcess GetActivatingProcess()
        {
            return new ActivatingProcess(Data);
        }
    }
}