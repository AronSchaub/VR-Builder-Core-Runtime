// Copyright (c) 2026 Aron Schaub
// SPDX-License-Identifier: Apache-2.0

using VRBuilder.Core.Primitives;

namespace VRBuilder.Core.Properties
{
    public interface IScaleProperty : ISceneObjectProperty
    {
        /// <summary>
        /// Scales the object to the given scale.
        /// </summary>
        /// <param name="targetScale"></param>
        /// <param name="progress">value between 0 and 1</param>
        /// <param name="animationCurve">can be null. Implementations will use progress as is and interpolate linear</param>
        void ScaleTo(IVector3 targetScale, float progress, IAnimationCurve animationCurve = null);
    }
}