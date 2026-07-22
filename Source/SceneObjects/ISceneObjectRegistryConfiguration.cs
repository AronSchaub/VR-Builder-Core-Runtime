// Copyright (c) 2026 Aron Schaub
// SPDX-License-Identifier: Apache-2.0

using VRBuilder.Core.Helpers;
using VRBuilder.Core.Registry;

namespace VRBuilder.Core.SceneObjects
{
    public interface ISceneObjectRegistryConfiguration: IServiceConfiguration
    {
        ISceneObjectFinder SceneObjectFinder { get; }
        ISceneObjectIdentity SceneObjectIdentity { get; }
        IEditorPrefabHandler EditorPrefabHandler { get; }
    }
}