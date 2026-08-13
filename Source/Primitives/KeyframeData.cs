// Modifications copyright (c) 2026 Aron Schaub
// SPDX-License-Identifier: Apache-2.0

using System.Runtime.Serialization;

namespace VRBuilder.Core.Primitives
{
    /// <summary>
    /// Engine-agnostic, serializable keyframe used to define animation curves.
    /// Mirrors Unity's Keyframe struct without a Unity dependency.
    /// </summary>
    [DataContract]
    public struct KeyframeData : IKeyframe
    {
        /// <summary>The time at which the keyframe occurs on the curve.</summary>
        [DataMember]
        public float Time { readonly get; set; }

        /// <summary>The value of the curve at <see cref="Time"/>.</summary>
        [DataMember]
        public float Value { readonly get; set; }

        /// <summary>The tangent of the curve entering the keyframe.</summary>
        [DataMember]
        public float InTangent { readonly get; set; }

        /// <summary>The tangent of the curve leaving the keyframe.</summary>
        [DataMember]
        public float OutTangent { readonly get; set; }

        /// <summary>The weighted tangent mode of the keyframe, represented as an integer.</summary>
        [DataMember]
        public int WeightedMode { readonly get; set; }
        
        /// <summary>The incoming weight affects the slope of the curve from the previous key to this key.</summary>
        [DataMember]
        public float InWeight { readonly get; set; }
        
        /// <summary>The outgoing weight affects the slope of the curve from this key to the next key.</summary>
        [DataMember]
        public float OutWeight { readonly get; set; }

        /// <summary>
        /// Initializes a new <see cref="KeyframeData"/> with the specified values.
        /// </summary>
        /// <param name="time">The time at which the keyframe occurs on the curve.</param>
        /// <param name="value">The value of the curve at <paramref name="time"/>.</param>
        /// <param name="inTangent">The tangent of the curve entering the keyframe. Defaults to 0.</param>
        /// <param name="outTangent">The tangent of the curve leaving the keyframe. Defaults to 0.</param>
        /// <param name="weightedMode">The weighted tangent mode of the keyframe. Defaults to 0.</param>
        /// <param name="inWeight">The incoming weight affects the slope of the curve. Defaults to 0.</param>
        /// <param name="outWeight">The outgoing weight affects the slope of the curve. Defaults to 0.</param>
        public KeyframeData(float time, float value, float inTangent = 0f, float outTangent = 0f, int weightedMode = 0, float inWeight = 0f, float outWeight = 0f)
        {
            Time = time;
            Value = value;
            InTangent = inTangent;
            OutTangent = outTangent;
            WeightedMode = weightedMode;
            InWeight = inWeight;
            OutWeight = outWeight;
        }
    }
}