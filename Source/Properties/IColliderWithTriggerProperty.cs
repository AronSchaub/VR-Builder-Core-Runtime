// copyright (c) 2026 Aron Schaub
// SPDX-License-Identifier: Apache-2.0

using VRBuilder.Core.SceneObjects;

namespace VRBuilder.Core.Properties
{
    public interface IColliderWithTriggerProperty : ISceneObjectProperty
    {
        bool IsTransformInsideTrigger(ISceneObject sceneObject);
        void FastForwardEnter(ISceneObject objs);
    }
}