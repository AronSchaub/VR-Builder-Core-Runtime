// Copyright (c) 2026 Aron Schaub
// SPDX-License-Identifier: Apache-2.0

namespace VRBuilder.Core.StepLocking
{
    public static class StepLockLocator
    {
        private static IStepLockService? current;
        public static IStepLockService? Current { get; set; }
        public static bool IsRegistered => current != null;
    }
}