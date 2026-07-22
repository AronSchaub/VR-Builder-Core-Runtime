// Copyright (c) 2026 Aron Schaub
// SPDX-License-Identifier: Apache-2.0

using VRBuilder.Core.Registry;

namespace VRBuilder.Core.StepLocking
{
    public interface IStepLockConfiguration : IServiceConfiguration
    {
        bool LockOnProcessStart { get; set; } // = true;
        bool LockOnProcessFinished { get; set; } // = true;
    }
}