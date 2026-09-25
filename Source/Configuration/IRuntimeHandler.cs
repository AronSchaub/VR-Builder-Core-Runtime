// Copyright (c) 2026 Aron Schaub
// SPDX-License-Identifier: Apache-2.0

using System;

namespace VRBuilder.Core.Configuration
{
    /// <summary>
    /// Implemented by components that expose the runtime configuration used to start a process.
    /// </summary>
    public interface IRuntimeHandler
    {
        /// <summary>
        /// Get the current or default configuration that determines which process is selected and where its manifest is located.
        /// </summary>
        IRuntimeConfiguration RuntimeConfiguration { get; set; }
        
        /// <summary>
        /// Current selected process referenced by <see cref="IRuntimeConfiguration"/> and trigger <see cref="SelectedProcessChanged"/> on set.
        /// </summary>
        string SelectedProcess { get; set; }

        /// <summary>
        /// Event raised if the process of the <see cref="IRuntimeConfiguration"/> is changed. The string argument is the selected process with the path.
        /// </summary>
        public event Action<string> SelectedProcessChanged;
    }
}