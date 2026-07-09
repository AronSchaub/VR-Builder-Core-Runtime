// copyright (c) 2026 Aron Schaub
// SPDX-License-Identifier: Apache-2.0

using VRBuilder.Core.SceneObjects;

namespace VRBuilder.Core.Properties
{
    public interface ITransformInRangeDetectorProperty : ISceneObjectProperty
    {
        float DetectionRange { get; set; }
        bool IsTargetInsideRange();
        void SetTrackedTransform(ISceneObject transformToBeTracked);
        void ForceMoveToTracked();
    }
}