#if UNITY_2022_OR_NEWER
using UnityEngine;
#elif GODOT
using Godot;
using System.Diagnostics;
#endif

namespace VRBuilder.Core {

    /// <summary>
    /// this class will forward the logging to either Unity or Godot
    /// </summary>
    public static class ForwardingLogger
    {
    #if UNITY_2022_OR_NEWER
        [HideInCallstack]
        public static void Log(object message) => Debug.Log(message);
        [HideInCallstack]
        public static void LogFormat(string format, params object[] args) => Debug.LogFormat(format, args);
        [HideInCallstack]
        public static void LogWarning(object message) => Debug.LogWarning(message);
        [HideInCallstack]
        public static void LogWarningFormat(string format, params object[] args) => Debug.LogWarningFormat(format, args);
        [HideInCallstack]
        public static void LogError(object message) => Debug.LogError(message);
        [HideInCallstack]
        public static void LogErrorFormat(string format, params object[] args) => Debug.LogErrorFormat(format, args);
        [HideInCallstack]
        public static void LogAssertion(object message) => Debug.LogAssertion(message);
        [HideInCallstack]
        public static void LogAssertionFormat(string format, params object[] args) => Debug.LogAssertionFormat(format, args);
        [HideInCallstack]
        public static void LogException(System.Exception exception) => Debug.LogException(exception);
    #elif GODOT
        [StackTraceHidden]
        public static void Log(object message) => GD.Print(message);
        [StackTraceHidden]
        public static void LogFormat(string format, params object[] args) => GD.Print(string.Format(format, args));
        [StackTraceHidden]
        public static void LogWarning(object? message) => GD.PushWarning(message?.ToString() ?? string.Empty);
        [StackTraceHidden]
        public static void LogWarningFormat(string format, params object[] args) => GD.PushWarning(string.Format(format, args));
        [StackTraceHidden]
        public static void LogError(object? message) => GD.PushError(message?.ToString() ?? string.Empty);
        [StackTraceHidden]
        public static void LogErrorFormat(string format, params object[] args) => GD.PushError(string.Format(format, args));
        [StackTraceHidden]
        public static void LogAssertion(object? message) => GD.PushError($"Assertion failed: {message}");
        [StackTraceHidden]
        public static void LogAssertionFormat(string format, params object[] args) => GD.PushError($"Assertion failed: {string.Format(format, args)}");
        [StackTraceHidden]
        public static void LogException(System.Exception exception) => GD.PushError(exception.ToString());
    #else
        // Fallback for unknown or unsupported platforms
        public static void Log(object message) { }
        public static void LogFormat(string format, params object[] args) { }
        public static void LogWarning(object message) { }
        public static void LogWarningFormat(string format, params object[] args) { }
        public static void LogError(object message) { }
        public static void LogErrorFormat(string format, params object[] args) { }
        public static void LogAssertion(object message) { }
        public static void LogAssertionFormat(string format, params object[] args) { }
        public static void LogException(System.Exception exception) { }
    #endif
    }
}