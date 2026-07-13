// Modifications copyright (c) 2026 Aron Schaub
// SPDX-License-Identifier: Apache-2.0

using System.Runtime.Serialization;
using VRBuilder.Core.Primitives;

namespace VRBuilder.Core.Properties
{
    [DataContract]
    public struct Vector2Data : IVector2
    {
        [DataMember]
        public float X { readonly get; set; }

        [DataMember]
        public float Y { readonly get; set; }

        public Vector2Data(float x, float y)
        {
            X = x;
            Y = y;
        }
    }
}