// Copyright (c) 2026 Aron Schaub
// SPDX-License-Identifier: Apache-2.0

using System;

namespace VRBuilder.Core
{
    /// <summary>
    /// Engine-agnostic logger. Delegates are set by the engine-specific
    /// initializer in Core during startup. No Unity/Godot dependencies.
    /// </summary>
    public static class ForwardingLogger
    {
        public static Action<object> LogAction { get; set; }
        public static Action<string, object[]> LogFormatAction { get; set; }
        public static Action<object> LogWarningAction { get; set; }
        public static Action<string, object[]> LogWarningFormatAction { get; set; }
        public static Action<object> LogErrorAction { get; set; }
        public static Action<string, object[]> LogErrorFormatAction { get; set; }
        public static Action<object> LogAssertionAction { get; set; }
        public static Action<string, object[]> LogAssertionFormatAction { get; set; }
        public static Action<Exception> LogExceptionAction { get; set; }

        public static void Log(object message) => LogAction?.Invoke(message);
        public static void LogFormat(string format, params object[] args) => LogFormatAction?.Invoke(format, args);
        public static void LogWarning(object message) => LogWarningAction?.Invoke(message);
        public static void LogWarningFormat(string format, params object[] args) => LogWarningFormatAction?.Invoke(format, args);
        public static void LogError(object message) => LogErrorAction?.Invoke(message);
        public static void LogErrorFormat(string format, params object[] args) => LogErrorFormatAction?.Invoke(format, args);
        public static void LogAssertion(object message) => LogAssertionAction?.Invoke(message);
        public static void LogAssertionFormat(string format, params object[] args) => LogAssertionFormatAction?.Invoke(format, args);
        public static void LogException(Exception exception) => LogExceptionAction?.Invoke(exception);
    }
}