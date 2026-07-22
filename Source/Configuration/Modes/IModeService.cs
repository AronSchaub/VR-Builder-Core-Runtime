// Copyright (c) 2013-2019 Innoactive GmbH
// Licensed under the Apache License, Version 2.0
// Modifications copyright (c) 2021-2026 MindPort GmbH
// Modifications copyright (c) 2026 Aron Schaub
// SPDX-License-Identifier: Apache-2.0

using System;
using VRBuilder.Core.Registry;

namespace VRBuilder.Core.Configuration.Modes
{
    /// <summary>
    /// The interface of a process mode. A process mode determines if an entity has to be skipped and provides configurable entities with parameters.
    /// </summary>
    public interface IModeService
    {
        /// <summary>
        /// The name of this process mode.
        /// </summary>
        string Name { get; }
        
        /// <summary>
        /// Get or set the default or active mode.
        /// </summary>
        public IModeService ActiveOrDefaultMode { get; set; }
        
        /// <summary>
        /// Returns whether the given <see cref="IOptional"/> type should be skipped in this process mode.
        /// </summary>
        /// <typeparam name="ISkippable">The actual type implementing ISkippable.</typeparam>
        bool CheckIfSkipped<TOptional>() where TOptional : IOptional;

        /// <summary>
        /// Returns whether the given type should be skipped in this process mode.
        /// </summary>
        /// <param name="type">The type to check.</param>
        bool CheckIfSkipped(Type type);

        /// <summary>
        /// Provides a specific parameter for this mode.
        /// </summary>
        /// <param name="key">Name of the parameter.</param>
        /// <typeparam name="T">Type this parameter should be.</typeparam>
        /// <returns>The value for the given key</returns>
        T GetParameter<T>(string key);

        /// <summary>
        /// Checks if given key exists.
        /// </summary>
        /// <param name="key">Name of the key</param>
        bool ContainsParameter<T>(string key);
    }
}
