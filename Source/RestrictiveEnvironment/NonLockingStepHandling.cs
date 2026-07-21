// Copyright (c) 2013-2019 Innoactive GmbH
// Modifications copyright (c) 2021-2026 MindPort GmbH
// Modifications copyright (c) 2026 Aron Schaub
// SPDX-License-Identifier: Apache-2.0

using System.Collections.Generic;
using System.Linq;
using VRBuilder.Core.Configuration.Modes;
using VRBuilder.Core.StepLocking;

namespace VRBuilder.Core.RestrictiveEnvironment
{
    /// <summary>
    /// This implementation does not care about restrictive environment and does nothing.
    /// Use this strategy to disable the feature.
    /// </summary>
    public class NonLockingStepHandling : IStepLockService
    {
        /// <inheritdoc />
        public void Unlock(IStepData data, IEnumerable<LockablePropertyData> manualUnlocked)
        {
        }

        /// <inheritdoc />
        public void Lock(IStepData data, IEnumerable<LockablePropertyData> manualUnlocked)
        {
        }

        public void Configure(IModeService modeService)
        {
        }

        public void OnProcessStarted(IProcess process)
        {
        }

        public void OnProcessFinished(IProcess process)
        {
        }

        public void SetConfiguration(object configuration)
        {
        }

        public void Initialize()
        {
        }
    }
}