// Copyright (c) 2026 Aron Schaub
// SPDX-License-Identifier: Apache-2.0

using VRBuilder.Core.Registry;

namespace VRBuilder.Core.ProcessRunning
{
    /// <summary>
    /// Contract for running a process.
    /// This is the service-oriented counterpart to the static <see cref="DefaultProcessRunner"/>.
    /// The default implementation is registered automatically in the service registry.
    /// </summary>
    public interface IProcessRunner : IService<IProcessRunnerConfiguration>
    {
        /// <summary>
        /// The currently running process, or <c>null</c> if none is running.
        /// </summary>
        IProcess CurrentProcess { get; }

        /// <summary>
        /// The current Chapter, or <c>null</c> if none is running.
        /// </summary>
        IChapter CurrentChapter { get; }

        /// <summary>
        /// The currently running process, or <c>null</c> if none is running.
        /// </summary>
        IStep CurrentStep { get; }

        /// <summary>
        /// <c>true</c> if a process has been initialized and is currently active.
        /// </summary>
        bool IsRunning { get; }

        /// <summary>
        /// Lifecycle events for the current process.
        /// These mirror the events on <see cref="ProcessEvents"/>.
        /// </summary>
        ProcessEvents Events { get; }

        /// <summary>
        /// Initializes the runner with a process, creating required scene components.
        /// </summary>
        /// <param name="process">The process to run.</param>
        void Initialize(IProcess process);

        /// <summary>
        /// Starts execution of the initialized process.
        /// </summary>
        void Start();

        void Update();

        void Stop();

        /// <summary>
        /// Sets the specified chapter as the next chapter to execute.
        /// </summary>
        void SetNextChapter(IChapter chapter);

        /// <summary>
        /// Skips the current step using the given transition to fast-forward.
        /// </summary>
        void SkipStep(ITransition transition);

        /// <summary>
        /// Skips the given number of chapters ahead.
        /// </summary>
        void SkipChapters(int numberOfChapters);

        /// <summary>
        /// Skips the current chapter entirely.
        /// </summary>
        void SkipCurrentChapter();

        void OnSceneUnloaded(string sceneName);
    }
}