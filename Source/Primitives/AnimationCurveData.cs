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

        [DataMember]
        public int PreWrapMode { readonly get; set; }

        [DataMember]
        public int PostWrapMode { readonly get; set; }

        public AnimationCurveData(KeyframeData[] keyframes, int preWrapMode = 0, int postWrapMode = 0)
        {
            Keyframes = keyframes;
            PreWrapMode = preWrapMode;
            PostWrapMode = postWrapMode;
        }

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
    }
}