// Modifications copyright (c) 2026 Aron Schaub
// SPDX-License-Identifier: Apache-2.0

using System.Runtime.Serialization;
using VRBuilder.Core.Primitives;

namespace VRBuilder.Core.Properties
{
    [DataContract]
    public struct Vector4Data : IVector3
    {
        [DataMember]
        public float X { readonly get; set; }

        [DataMember]
        public float Y { readonly get; set; }

        [DataMember]
        public float Z { readonly get; set; }
        
        [DataMember]
        public float W { readonly get; set; }
        
        public Vector4Data(float x, float y, float z, float w)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }
        
        public static Vector3Data LerpUnclamped(Vector3Data a, Vector3Data b, float t)
        {
            return new Vector3Data(
                a.X + (b.X - a.X) * t,
                a.Y + (b.Y - a.Y) * t,
                a.Z + (b.Z - a.Z) * t
            );
        }
        
        public static Vector3Data One => new Vector3Data(1f, 1f, 1f);

    }
}