// Modifications copyright (c) 2026 Aron Schaub
// SPDX-License-Identifier: Apache-2.0

using System.Collections;
using System.Diagnostics;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using VRBuilder.Core.Attributes;

namespace VRBuilder.Core.Behaviors
{
    /// <summary>
    /// Behavior that waits for `DelayTime` seconds before finishing its activation.
    /// </summary>
    [DataContract(IsReference = true)]
    [HelpLink("https://www.mindport.co/vr-builder/manual/default-behaviors/delay")]
    public class DelayBehavior : Behavior<DelayBehavior.EntityData>
    {
        /// <summary>
        /// The data class for a delay behavior.
        /// </summary>
        [DisplayName("Delay")]
        [DataContract(IsReference = true)]
        public class EntityData : IBehaviorData
        {
            [DataMember]
            [DisplayName("Delay (in seconds)")]
            public float DelayTime { get; set; }

            public Metadata Metadata { get; set; }

            [IgnoreDataMember]
            public string Name
            {
                get { return $"Wait for {DelayTime} seconds"; }
            }
        }

        [JsonConstructor]
        public DelayBehavior() : this(0)
        {
        }

        public DelayBehavior(float delayTime)
        {
            if (delayTime < 0f)
            {
                ForwardingLogger.LogWarningFormat("DelayTime has to be zero or positive, but it was {0}. Setting to 0 instead.", delayTime);
                delayTime = 0f;
            }

            Data.DelayTime = delayTime;
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
                while (stopWatch.ElapsedMilliseconds < Data.DelayTime)
                    yield return null;
            }

            /// <inheritdoc />
            public override void End()
            {
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