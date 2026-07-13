// Copyright (c) 2026 Aron Schaub
// SPDX-License-Identifier: Apache-2.0

using VRBuilder.Core.Primitives;

namespace VRBuilder.Core.Properties
{
    public interface IScaleProperty : ISceneObjectProperty
    {
        void ScaleTo(IVector3 targetScale, float progress);
    }
}