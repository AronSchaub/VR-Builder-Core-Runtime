// Copyright (c) 2026 Aron Schaub
// SPDX-License-Identifier: Apache-2.0

using System;
using System.Runtime.Serialization;

namespace VRBuilder.Core.Primitives
{
    [DataContract]
    public struct AnimationCurveData : IAnimationCurve
    {
        [DataMember]
        public KeyframeData[] Keyframes { readonly get; set; }

        public static AnimationCurveData Linear(float timeStart, float valueStart, float timeEnd, float valueEnd)
        {
            return new AnimationCurveData
            {
                Keyframes = new[]
                {
                    new KeyframeData(timeStart, valueStart),
                    new KeyframeData(timeEnd, valueEnd)
                }
            };
        }

        [Obsolete("You have to convert this to your Platform specific AnimationCurve")]
        public float Evaluate(float time)
        {
            throw new NotImplementedException("You have to convert this to your Platform specific AnimationCurve");
        }
    }
}
