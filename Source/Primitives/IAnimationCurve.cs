// Copyright (c) 2026 Aron Schaub
// SPDX-License-Identifier: Apache-2.0

namespace VRBuilder.Core.Primitives
{
    public interface IAnimationCurve
    {
        float Evaluate(float time);
    }
}