// Copyright (c) 2026 Aron Schaub, Sebastian Pötter
// SPDX-License-Identifier: EUPL-1.2

using System;
using System.Collections.Generic;
using VRBuilder.Core.Registry;
using VRBuilder.Core.Utils.Audio;

namespace VRBuilder.Core.Configuration
{
    /// <summary>
    /// Handles configuration specific to this scene.
    /// </summary>
    public interface ISceneService : IService<ISceneConfiguration>
    {
        /// <summary>
        /// The scene handler of the current session.
        /// </summary>
        ISceneHandler? Handler { get; set; }

        /// <summary>
        /// Lists all assemblies whose property extensions will be used in the current scene.
        /// </summary>
        IEnumerable<string> ExtensionAssembliesWhitelist { get; }

        /// <summary>
        /// True if the specified type is not in an exclusion list for the specified assembly.
        /// </summary>
        bool IsAllowedInAssembly(Type extensionType, string assemblyName);

        /// <summary>
        /// Default resources prefab to use for Confetti behavior.
        /// </summary>
        string DefaultConfettiPrefab { get; set; }

        /// <summary>
        /// Default audio source for playing audio inside the scene.
        /// </summary>
        IAudioData DefaultAudioSource { get; set; }

        /// <summary>
        /// Adds the specified assembly names to the extension whitelist.
        /// </summary>
        public void AddWhitelistAssemblies(IEnumerable<string> assemblyNames);
    }
}