// Modifications copyright (c) 2026 Aron Schaub
// SPDX-License-Identifier: Apache-2.0

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using VRBuilder.Core.Attributes;
using VRBuilder.Core.Primitives;
using VRBuilder.Core.Properties;
using VRBuilder.Core.SceneObjects;

namespace VRBuilder.Core.Behaviors
{
    // This behavior linearly changes scale of a Target object over Duration seconds, until it matches TargetScale.
    [DataContract(IsReference = true)]
    public class ScalingBehavior : Behavior<ScalingBehavior.EntityData>
    {
        [DisplayName("Scale Object")]
        [DataContract(IsReference = true)]
        public class EntityData : IBehaviorData
        {
            // Process object to scale.
            [DataMember]
            [DisplayName("Target Objects")]
            public MultipleScenePropertyReference<IScaleProperty> Targets { get; set; }

            // Target scale.
            [DataMember]
            [DisplayName("Target Scale")]
            public IVector3 TargetScale { get; set; }

            // Duration of the animation in seconds.
            [DataMember]
            [DisplayName("Animation Duration (in seconds)")]
            public float Duration { get; set; }

            [DataMember]
            [DisplayName("Animation curve")]
            public IAnimationCurve AnimationCurve { get; set; }

            public Metadata Metadata { get; set; }

            /// <inheritdoc />
            [IgnoreDataMember]
            public string Name => $"Scale {Targets} to {TargetScale}";
        }

        [JsonConstructor]
        public ScalingBehavior() : this(Array.Empty<ISceneObject>(), Vector3Data.One, 0f)
        {
        }

        public ScalingBehavior(IEnumerable<ISceneObject> targets, IVector3 targetScale, float duration)
        {
            Data.Targets = new MultipleScenePropertyReference<IScaleProperty>(targets.Select(target => target.Guid));
            Data.TargetScale = targetScale;
            Data.Duration = duration;
            Data.AnimationCurve = AnimationCurveData.Linear(0f, 0f, 1f, 1f);
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
            }

            /// <inheritdoc />
            public override IEnumerator Update()
            {
                if (!Data.Targets.HasValue())
                {
                    throw new InvalidOperationException("ScalingBehavior: No target objects assigned to scale.");
                }

                if (!Data.Targets.Values.Any())
                {
                    yield break;
                }

                while (stopWatch.ElapsedMilliseconds < Data.Duration)
                {
                    float progress = stopWatch.ElapsedMilliseconds / Data.Duration;
                    float curve = Data.AnimationCurve.Evaluate(progress);

                    foreach (IScaleProperty property in Data.Targets.Values)
                    {
                        property.ScaleTo(Data.TargetScale, curve);
                    }

                    yield return null;
                }
            }

            /// <inheritdoc />
            public override void End()
            {
                foreach (var property in Data.Targets.Values)
                {
                    property.ScaleTo(Data.TargetScale, 1f);
                }

                stopWatch.Stop();
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