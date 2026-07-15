// Modifications copyright (c) 2026 Aron Schaub
// SPDX-License-Identifier: Apache-2.0

namespace VRBuilder.Core.Primitives
{
    public interface IKeyframe
    {
        float Time { get; }
        float Value { get; }
        float InTangent { get; }
        float OutTangent { get; }
        int WeightedMode { get; set; }
    }
}