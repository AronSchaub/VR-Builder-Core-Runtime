using System;
using System.Collections.Generic;
using VRBuilder.Core.Configuration;
using VRBuilder.UI.Console;

namespace VRBuilder.Core.Utils
{
    /// <summary>
    /// Log messages to a in-world console set up in the <see cref="RuntimeConfigurator"/>.
    /// </summary>
    public static class WorldConsole
    {
        private static ILogConsole console;
        private static Queue<Action> executionQueue = new Queue<Action>();

        static WorldConsole()
        {
            console = RuntimeConfigurator.Configuration.WorldConsole;

            if (console == null)
            {
                ForwardingLogger.LogError("Could not initialize world console.");
                return;
            }

            console.Hide();
        }

        /// <summary>
        /// Processes the queued log messages thus showing them on the console.
        /// </summary>
        public static void Refresh()
        {
            if (console is null)
            {
                return;
            }

            lock (executionQueue)
            {
                while (executionQueue.Count > 0)
                {
                    executionQueue.Dequeue()?.Invoke();
                }
            }
        }

        /// <summary>
        /// Logs a message in the console.
        /// </summary>
        /// <param name="message">The message to be logged.</param>
        /// <param name="details">Additional details to show when expanding the message.</param>
        /// <param name="show">If true, show the console when the message is logged.</param>
        public static void Log(string message, string details = "", bool show = false)
        {
            lock (executionQueue)
            {
                executionQueue.Enqueue(() =>
                {
                    console.LogMessage(message, details, LogLevel.Info);

                    if (show)
                    {
                        console.Show();
                    }
                });
            }

            console.SetDirty();
        }


        /// <summary>
        /// Logs a warning in the console.
        /// </summary>
        /// <param name="message">The message to be logged.</param>
        /// <param name="details">Additional details to show when expanding the message.</param>
        /// <param name="show">If true, show the console when the message is logged.</param>
        public static void LogWarning(string message, string details = "", bool show = false)
        {
            lock (executionQueue)
            {
                executionQueue.Enqueue(() =>
                {
                    console.LogMessage(message, details, LogLevel.Warn);

                    if (show)
                    {
                        console.Show();
                    }
                });
            }

            console.SetDirty();
        }


        /// <summary>
        /// Logs an error in the console.
        /// </summary>
        /// <param name="message">The message to be logged.</param>
        /// <param name="details">Additional details to show when expanding the message.</param>
        /// <param name="show">If true, show the console when the message is logged.</param>
        public static void LogError(string message, string details = "", bool show = true)
        {
            lock (executionQueue)
            {
                executionQueue.Enqueue(() =>
                {
                    console.LogMessage(message, details, LogLevel.Error);

                    if (show)
                    {
                        console.Show();
                    }
                });
            }

            console.SetDirty();
        }

        /// <summary>
        /// Logs an exception in the console.
        /// </summary>
        /// <param name="ex">The exception to log.</param>
        /// <param name="show">If true, show the console when the message is logged.</param>
        public static void LogException(Exception ex, bool show = true)
        {
            lock (executionQueue)
            {
                executionQueue.Enqueue(() =>
                {
                    console.LogMessage(ex.Message, ex.StackTrace, LogLevel.Error);

                    if (show)
                    {
                        console.Show();
                    }
                });
            }

            console.SetDirty();
        }
    }
}
