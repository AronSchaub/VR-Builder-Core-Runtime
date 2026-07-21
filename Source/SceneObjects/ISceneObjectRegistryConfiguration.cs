// Copyright (c) 2026 Aron Schaub
// SPDX-License-Identifier: Apache-2.0

using VRBuilder.Core.Helpers;

namespace VRBuilder.Core.SceneObjects
{
    public interface ISceneObjectRegistryConfiguration
    {
        ISceneObjectFinder SceneObjectFinder { get; }
        ISceneObjectIdentity SceneObjectIdentity { get; }
        IEditorPrefabHandler EditorPrefabHandler { get; }
    }
}