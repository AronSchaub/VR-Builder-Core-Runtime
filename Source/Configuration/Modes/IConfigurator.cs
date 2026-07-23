// Copyright (c) 2013-2019 Innoactive GmbH
// Modifications copyright (c) 2021-2026 MindPort GmbH
// SPDX-License-Identifier: Apache-2.0

namespace VRBuilder.Core.Configuration.Modes
{
    /// <summary>
    /// An interface for entities' configurators.
    /// </summary>
    public interface IConfigurator
    {
        /// <summary>
        /// Configures the entity based on the given mode and stage.
        /// </summary>
        /// <param name="modeService">The current mode.</param>
        /// <param name="stage">The current stage of the entity.</param>
        void Configure(IModeService modeService, Stage stage);
    }
}