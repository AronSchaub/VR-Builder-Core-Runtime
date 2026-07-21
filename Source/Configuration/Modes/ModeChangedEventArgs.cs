// Copyright (c) 2013-2019 Innoactive GmbH
// Licensed under the Apache License, Version 2.0
// Modifications copyright (c) 2021-2026 MindPort GmbH

using System;

namespace VRBuilder.Core.Configuration.Modes
{
    /// <summary>
    /// This is a <see cref="EventArgs"/> used for <see cref="IModeService"/> changes.
    /// If you want so see more about EventArgs, please visit: https://docs.microsoft.com/en-us/dotnet/standard/events/
    /// </summary>
    public class ModeChangedEventArgs : EventArgs
    {
        /// <summary>
        /// The newly activated <see cref="IModeService"/>.
        /// </summary>
        public IModeService ModeService { get; private set; }

        public ModeChangedEventArgs(IModeService modeService)
        {
            ModeService = modeService;
        }
    }
}
