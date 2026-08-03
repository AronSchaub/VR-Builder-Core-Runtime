// Copyright (c) 2013-2019 Innoactive GmbH
// Licensed under the Apache License, Version 2.0
// Modifications copyright (c) 2021-2026 MindPort GmbH

namespace VRBuilder.Core.Configuration.Modes
{
    /// <summary>
    /// Data that carries the <see cref="IModeService"/> which determines how its owner behaves.
    /// </summary>
    public interface IModeData : IData
    {
        /// <summary>
        /// The mode service that drives which <see cref="IOptional"/> parts are skipped and which parameters are applied.
        /// </summary>
        IModeService ModeService { get; set; }
    }
}