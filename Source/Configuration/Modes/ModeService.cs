// Copyright (c) 2013-2019 Innoactive GmbH
// Modifications copyright (c) 2021-2026 MindPort GmbH
// Modifications copyright (c) 2026 Aron Schaub
// SPDX-License-Identifier: Apache-2.0

using System;
using System.Collections.Generic;
using System.Linq;

namespace VRBuilder.Core.Configuration.Modes
{
    /// <summary>
    /// A process mode that is defined by its name, IConfigurables activation policy and a collection of parameters.
    /// Immutable.
    /// </summary>
    public sealed class ModeService : IModeService
    {
        /// <summary>
        /// A rule that determines which <see cref="IOptional"/> implementations have to be skipped.
        /// </summary>
        private readonly TypeRule<IOptional> entitiesToSkip;

        private readonly Dictionary<string, object> parameters;

        /// <summary>
        /// Initializes a <see cref="ModeService"/> with the default name and a whitelist type rule that skips nothing.
        /// The created instance is assigned to <see cref="ActiveOrDefaultMode"/>.
        /// </summary>
        public ModeService() : this("Default", new WhitelistTypeRule<IOptional>())
        {
            ActiveOrDefaultMode = this;
        }

        /// <summary>
        /// Initializes a <see cref="ModeService"/> with the given name, skip rule and parameters.
        /// The parameters are copied into a new dictionary so the original collection cannot be modified after construction.
        /// </summary>
        /// <param name="name">Name of the process mode.</param>
        /// <param name="entitiesToSkip">A type rule which determines if an <see cref="IOptional"/> has to be skipped, depending on its type.</param>
        /// <param name="parameters">A string-to-object dictionary of process mode parameters.</param>
        public ModeService(string name, TypeRule<IOptional> entitiesToSkip, Dictionary<string, object> parameters = null)
        {
            Name = name;
            this.entitiesToSkip = entitiesToSkip;

            parameters ??= new Dictionary<string, object>();
            this.parameters = parameters.ToDictionary(entry => entry.Key, entry => entry.Value);
        }

        /// <inheritdoc />
        public string Name { get; private set; }

        /// <inheritdoc />
        public IModeService ActiveOrDefaultMode { get; set; }

        /// <inheritdoc />
        public IModeHandler ModeHandler { get; set; }

        /// <inheritdoc />
        public bool CheckIfSkipped<TSkippable>() where TSkippable : IOptional
        {
            return CheckIfSkipped(typeof(TSkippable));
        }

        /// <inheritdoc />
        public bool CheckIfSkipped(Type type)
        {
            return entitiesToSkip.IsQualifiedBy(type);
        }

        /// <inheritdoc />
        public TValue GetParameter<TValue>(string key)
        {
            return (TValue)parameters[key];
        }

        /// <inheritdoc />
        public bool ContainsParameter<TValue>(string key)
        {
            return parameters.ContainsKey(key) && parameters[key] is TValue;
        }
    }
}