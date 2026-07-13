// Copyright (c) 2026 Aron Schaub
// SPDX-License-Identifier: Apache-2.0

namespace VRBuilder.Core.Properties
{
    public interface ISceneProperty : ISceneObjectProperty
    {
        IAsyncCallback StartLoadAsync(string scenePath, bool loadAdditively);
        void LoadSynchronously(string scenePath, bool loadAdditively);
    }

    public interface IAsyncCallback
    {
        bool isDone { get; }
    }
}