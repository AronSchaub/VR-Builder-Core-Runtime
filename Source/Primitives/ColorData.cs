// Copyright (c) 2021-2026 MindPort GmbH
// Licensed under the Apache License, Version 2.0

using System;

namespace VRBuilder.Core.Primitives
{
    /// <summary>
    /// Simple concrete implementation of <see cref="IColor"/> for use in CoreRuntime.
    /// Conversion to/from UnityEngine.Color happens in the Core layer via extension methods.
    /// </summary>
    public struct ColorData : IColor, IEquatable<ColorData>
    {
        /// <inheritdoc/>
        public float R { get; }

        /// <inheritdoc/>
        public float G { get; }

        /// <inheritdoc/>
        public float B { get; }

        /// <inheritdoc/>
        public float A { get; }

        /// <summary>
        /// Creates a new ColorData from float components (0..1 range).
        /// </summary>
        public ColorData(float r, float g, float b, float a)
        {
            R = r;
            G = g;
            B = b;
            A = a;
        }

        /// <summary>
        /// Creates a new ColorData from float components with alpha = 1.
        /// </summary>
        public ColorData(float r, float g, float b) : this(r, g, b, 1f)
        {
        }

        public bool Equals(ColorData other)
        {
            return R.Equals(other.R) && G.Equals(other.G) && B.Equals(other.B) && A.Equals(other.A);
        }

        public override bool Equals(object obj)
        {
            return obj is ColorData other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(R, G, B, A);
        }

        public static bool operator ==(ColorData left, ColorData right) => left.Equals(right);
        public static bool operator !=(ColorData left, ColorData right) => !left.Equals(right);

        public override string ToString()
        {
            return $"ColorData({R:F3}, {G:F3}, {B:F3}, {A:F3})";
        }
    }
}
