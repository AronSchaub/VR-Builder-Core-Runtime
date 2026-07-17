// Copyright (c) 2013-2019 Innoactive GmbH
// Licensed under the Apache License, Version 2.0
// Modifications copyright (c) 2021-2026 MindPort GmbH

#if UNITY_6000_0_OR_NEWER
using System;
#elif GODOT
using Godot;
#endif


namespace VRBuilder.Core.SceneObjects
{
#if UNITY_6000_0_OR_NEWER
    public class LockStateChangedEventArgs : EventArgs
#elif GODOT
    public partial class LockStateChangedEventArgs : GodotObject
#endif
    {
        public readonly bool IsLocked;

        public LockStateChangedEventArgs(bool isLocked)
        {
            IsLocked = isLocked;
        }
    }
}
