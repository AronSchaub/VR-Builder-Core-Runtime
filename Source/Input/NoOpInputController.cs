// Copyright (c) 2013-2019 Innoactive GmbH
// Licensed under the Apache License, Version 2.0
// Modifications copyright (c) 2021-2026 MindPort GmbH

using VRBuilder.Core.Registry;

namespace VRBuilder.Core.Input
{
    /// <summary>
    /// Central controller for input via the new Input System using C# events.
    /// </summary>
    public class NoOpInputController : IInputController
    {
        public void SetupInputActions()
        {
        }

        public void LoadInputActions()
        {
        }

        public bool UsesCustomKeyBindingAsset()
        {
            return false;
        }

        public void SetConfiguration(IInputConfiguration configuration)
        {
        }
    }
}