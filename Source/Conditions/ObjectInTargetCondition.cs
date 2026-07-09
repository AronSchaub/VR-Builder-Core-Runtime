// Modifications copyright (c) 2026 Aron Schaub
// SPDX-License-Identifier: Apache-2.0

using System;
using System.Collections;

namespace VRBuilder.Core.Conditions
{
    /// <summary>
    /// An active process for "object in target" conditions.
    /// </summary>
    public abstract class ObjectInTargetActiveProcess<TData> : StageProcess<TData> where TData : class, IObjectInTargetData
    {
        protected ObjectInTargetActiveProcess(TData data) : base(data)
        {
        }

        private bool isInside;
        private long timeStarted;

        /// <inheritdoc />
        public override void Start()
        {
            Data.IsCompleted = false;
            isInside = IsInside();

            if (isInside)
            {
                timeStarted = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            }
        }

        /// <summary>
        /// Returns true if the object is inside target.
        /// </summary>
        protected abstract bool IsInside();

        /// <inheritdoc />
        public override IEnumerator Update()
        {
            while (true)
            {
                if (isInside != IsInside())
                {
                    isInside = !isInside;

                    if (isInside)
                    {
                        timeStarted = DateTimeOffset.Now.ToUnixTimeMilliseconds();
                    }
                }

                if (isInside && DateTimeOffset.Now.ToUnixTimeMilliseconds() - timeStarted >= Data.RequiredTimeInside)
                {
                    Data.IsCompleted = true;
                    break;
                }

                yield return null;
            }
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
}