// Modifications copyright (c) 2026 Aron Schaub
// SPDX-License-Identifier: Apache-2.0

using System.Runtime.Serialization;

namespace VRBuilder.Core.Primitives
{
    [DataContract]
    public struct KeyframeData : IKeyframe
    {
        [DataMember]
        public float Time { readonly get; set; }

        [DataMember]
        public float Value { readonly get; set; }

        [DataMember]
        public float InTangent { readonly get; set; }

        [DataMember]
        public float OutTangent { readonly get; set; }

        public KeyframeData(float time, float value, float inTangent = 0f, float outTangent = 0f)
        {
            Time = time;
            Value = value;
            InTangent = inTangent;
            OutTangent = outTangent;
        }
    }
}