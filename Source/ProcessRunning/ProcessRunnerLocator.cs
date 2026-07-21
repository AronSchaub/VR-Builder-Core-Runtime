// Copyright (c) 2026 Aron Schaub
// SPDX-License-Identifier: Apache-2.0

namespace VRBuilder.Core.ProcessRunning
{
    /// <summary>
    /// Simple service locator for <see cref="IProcessRunner"/>.
    /// Allows engine-agnostic code to access the process runner without a direct dependency on Unity.
    /// The Unity-side registers itself via <c>[RuntimeInitializeOnLoadMethod]</c>.
    /// </summary>
    public static class ProcessRunnerLocator
    {
        private static IProcessRunner? current;

        /// <summary>
        /// The current <see cref="IProcessRunner"/> instance.
        /// </summary>
        public static IProcessRunner? Current
        {
            get => current;
            set => current = value;
        }

        /// <summary>
        /// True if a process runner has been registered.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.MemberNotNullWhen(true, nameof(Current))]
        public static bool IsRegistered => current != null;
    }
}