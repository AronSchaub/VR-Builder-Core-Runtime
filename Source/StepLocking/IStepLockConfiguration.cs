// Copyright (c) 2026 Aron Schaub
// SPDX-License-Identifier: Apache-2.0

namespace VRBuilder.Core.StepLocking
{
    public interface IStepLockConfiguration
    {
        string StepLockHandlingTypeName { get; }
        bool LockOnProcessStart { get; set; } // = true;
        bool LockOnProcessFinished { get; set; } // = true;
    }
}