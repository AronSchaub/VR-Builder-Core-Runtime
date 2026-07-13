// Modifications copyright (c) 2026 Aron Schaub
// SPDX-License-Identifier: Apache-2.0

using System.Diagnostics;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using VRBuilder.Core.Attributes;

namespace VRBuilder.Core.Conditions
{
    /// <summary>
    /// A condition that completes when a certain amount of time has passed.
    /// </summary>
    [DataContract(IsReference = true)]
    [HelpLink("https://www.mindport.co/vr-builder/manual/default-conditions/timeout-condition")]
    public class TimeoutCondition : Condition<TimeoutCondition.EntityData>
    {
        /// <summary>
        /// The data for timeout condition.
        /// </summary>
        [DisplayName("Timeout")]
        public class EntityData : IConditionData
        {
            /// <summary>
            /// The delay before the condition completes.
            /// </summary>
            [DataMember]
            [DisplayName("Wait (in seconds)")]
            public float Timeout { get; set; }

            /// <inheritdoc />
            public bool IsCompleted { get; set; }

            /// <inheritdoc />
            [IgnoreDataMember]
            [HideInProcessInspector]
            public string Name
            {
                get { return $"Complete after {Timeout} seconds"; }
            }

            /// <inheritdoc />
            public Metadata Metadata { get; set; }
        }

        private class ActiveProcess : BaseActiveProcessOverCompletable<EntityData>
        {
            private readonly Stopwatch stopWatch = new();

            public ActiveProcess(EntityData data) : base(data)
            {
            }

            public override void Start()
            {
                base.Start();
                stopWatch.Restart();
            }

            /// <inheritdoc />
            protected override bool CheckIfCompleted()
            {
                return stopWatch.ElapsedMilliseconds >= Data.Timeout;
            }

            /// <inheritdoc />
            public override void End()
            {
                base.End();
                stopWatch.Stop();
            }
        }

        [JsonConstructor]
        public TimeoutCondition() : this(0)
        {
        }

        public TimeoutCondition(float timeout)
        {
            Data.Timeout = timeout;
        }

        /// <inheritdoc />
        public override IStageProcess GetActiveProcess()
        {
            return new ActiveProcess(Data);
        }
    }
}