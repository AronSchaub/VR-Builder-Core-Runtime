// Modifications copyright (c) 2026 Aron Schaub
// SPDX-License-Identifier: Apache-2.0

using System.Runtime.Serialization;
using VRBuilder.Core.Primitives;

namespace VRBuilder.Core.Properties
{
    /// <summary>
    /// Engine-agnostic, serializable four-component vector.
    /// Implements <see cref="IVector4"/> so its X, Y,  Z, and W components can be consumed wherever a four-dimensional vector is expected.
    /// </summary>
    [DataContract]
    public struct Vector4Data : IVector4
    {
        /// <summary>The X component of the vector.</summary>
        [DataMember]
        public float X { readonly get; set; }

        /// <summary>The Y component of the vector.</summary>
        [DataMember]
        public float Y { readonly get; set; }

        /// <summary>The Z component of the vector.</summary>
        [DataMember]
        public float Z { readonly get; set; }

        /// <summary>The W component of the vector.</summary>
        [DataMember]
        public float W { readonly get; set; }

        /// <summary>
        /// Initializes a new <see cref="Vector4Data"/> with the specified components.
        /// </summary>
        /// <param name="x">The X component.</param>
        /// <param name="y">The Y component.</param>
        /// <param name="z">The Z component.</param>
        /// <param name="w">The W component.</param>
        public Vector4Data(float x, float y, float z, float w)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }

        /// <summary>
        /// Linearly interpolates between two <see cref="Vector3Data"/> values without clamping the interpolation factor.
        /// </summary>
        /// <param name="a">The start value.</param>
        /// <param name="b">The end value.</param>
        /// <param name="t">The interpolation factor; may be outside the 0..1 range.</param>
        /// <returns>The interpolated <see cref="Vector3Data"/>.</returns>
        public static Vector4Data LerpUnclamped(Vector4Data a, Vector4Data b, float t)
        {
            return new Vector4Data(
                a.X + (b.X - a.X) * t,
                a.Y + (b.Y - a.Y) * t,
                a.Z + (b.Z - a.Z) * t,
                a.W + (b.W - a.W) * t
            );
        }

        /// <summary>Gets a <see cref="Vector4Data"/> whose components are all 1.</summary>
        public static Vector4Data One => new Vector4Data(1f, 1f, 1f, 1f);
    }
}