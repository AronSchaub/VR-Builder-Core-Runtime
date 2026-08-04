// Copyright (c) 2013-2019 Innoactive GmbH
// Licensed under the Apache License, Version 2.0
// Modifications copyright (c) 2021-2026 MindPort GmbH

using System;
using VRBuilder.Core.Configuration.Modes;

namespace VRBuilder.Core.Serialization.V5
{
    /// <summary>
    /// Lightweight step reference used during V5 serialization to avoid duplicating step data.
    /// Replaces the nested V4 StepRef with a standalone class for cleaner $type output.
    /// </summary>
    internal class StepRef : IStep
    {
        IData IDataOwner.Data { get; } = null;

        IStepData IDataOwner<IStepData>.Data { get; } = null;

        public ILifeCycle LifeCycle { get; } = null;

        public IStageProcess GetActivatingProcess()
        {
            throw new NotImplementedException();
        }

        public IStageProcess GetActiveProcess()
        {
            throw new NotImplementedException();
        }

        public IStageProcess GetDeactivatingProcess()
        {
            throw new NotImplementedException();
        }

        public void Configure(IMode mode)
        {
            throw new NotImplementedException();
        }

        public void Update()
        {
            throw new NotImplementedException();
        }

        public IStep Clone()
        {
            throw new NotImplementedException();
        }

        public IStageProcess GetAbortingProcess()
        {
            throw new NotImplementedException();
        }

        public StepMetadata StepMetadata { get; set; }

        public IEntity Parent { get; set; }
    }
}