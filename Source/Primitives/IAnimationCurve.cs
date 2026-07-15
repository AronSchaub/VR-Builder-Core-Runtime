// Copyright (c) 2026 Aron Schaub
// SPDX-License-Identifier: Apache-2.0

namespace VRBuilder.Core.Primitives
{
    public interface IAnimationCurve
    {
        KeyframeData[] Keyframes { get; set; }
        int PreWrapMode { get; set; }
        int PostWrapMode { get; set; }
    }
}