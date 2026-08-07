// Copyright (c) 2013-2019 Innoactive GmbH
// Modifications copyright (c) 2021-2026 MindPort GmbH
// Modifications copyright (c) 2026 Aron Schaub
// SPDX-License-Identifier: Apache-2.0

namespace VRBuilder.Core.Configuration.Modes
{
    /// <summary>
    /// Service to locate the mode handler and the default or set mode in a session.
    /// Immutable.
    /// </summary>
    public sealed class ModeService : IModeService
    {
        public IMode ActiveOrDefaultMode
        {
            get {
                if (ModeHandler is null)
                {
                    ForwardingLogger.LogWarning("ModeHandler is not set. New default mode will be used.");
                    return new Mode("Default", new WhitelistTypeRule<IOptional>());
                }
                return ModeHandler.CurrentMode;
            }
            set
            {
                if (ModeHandler is null)
                {
                    ForwardingLogger.LogError("ModeHandler is not set. The mode can't be set.");
                }
                else
                {
                    ModeHandler.SetMode(value);
                }
            }
        }

        public IModeHandler ModeHandler { get; set; }
        
        private IModeServiceConfiguration configuration;

        public void SetConfiguration(IModeServiceConfiguration configuration)
        {
            this.configuration = configuration;
        }
    }
}