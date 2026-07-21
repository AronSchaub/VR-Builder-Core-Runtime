// Copyright (c) 2026 Aron Schaub
// SPDX-License-Identifier: Apache-2.0

namespace VRBuilder.Core.Multiplayer
{
    public static class MultiplayerServiceLocator
    {
        private static IMultiplayerService? current;
        public static IMultiplayerService? Current { get; set; }
        [System.Diagnostics.CodeAnalysis.MemberNotNullWhen(true, nameof(Current))]
        public static bool IsRegistered => current != null;
    }
}