// Copyright (c) 2026 Aron Schaub
// SPDX-License-Identifier: Apache-2.0

using System.Collections.Generic;
using System.Linq;
using VRBuilder.Core.Configuration.Modes;
using VRBuilder.Core.Runtime.Registry;
using VRBuilder.Core.StepLocking;

namespace VRBuilder.Core.ProcessRunning
{
    public class DefaultProcessRunner : IProcessRunner
    {
        private IProcessRunnerConfiguration configuration;
        private IProcess currentProcess;
        private ProcessEvents events;

        public IProcess CurrentProcess => currentProcess;
        public IChapter CurrentChapter => CurrentProcess?.Data.Current;
        public IStep CurrentStep => CurrentChapter?.Data.Current;
        public bool IsRunning => CurrentProcess != null && CurrentProcess.LifeCycle.Stage != Stage.Inactive;

        public ProcessEvents Events
        {
            get
            {
                events ??= new ProcessEvents();
                return events;
            }
        }

        public void Update()
        {
            if (currentProcess == null)
            {
                return;
            }

            if (currentProcess.LifeCycle.Stage == Stage.Inactive)
            {
                return;
            }

            Stage? currentChapterStage = currentProcess.Data.Current?.LifeCycle.Stage;
            Stage? currentStepStage = currentProcess.Data.Current?.Data.Current?.LifeCycle.Stage;

            currentProcess.Update();

            if (currentChapterStage.GetValueOrDefault() != Stage.Activating && currentProcess.Data.Current?.LifeCycle.Stage == Stage.Activating)
            {
                Events.ChapterStarted?.Invoke(this, new ProcessEventArgs(currentProcess));
            }

            if (currentStepStage.GetValueOrDefault() != Stage.Activating && currentProcess.Data.Current?.Data.Current?.LifeCycle.Stage == Stage.Activating)
            {
                Events.StepStarted?.Invoke(this, new ProcessEventArgs(currentProcess));
            }

            if (currentProcess.LifeCycle.Stage == Stage.Active)
            {
                currentProcess.LifeCycle.Deactivate();
                ServiceRegistry.Get<IStepLockService>()?.OnProcessFinished(currentProcess);
                Events.ProcessFinished?.Invoke(this, new ProcessEventArgs(currentProcess));
            }
        }

        public void SetConfiguration(IProcessRunnerConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void Initialize(IProcess process)
        {
            currentProcess = process;
            Events.ProcessInitialized?.Invoke(null, new ProcessEventArgs(process));
        }

        /// <summary>
        /// Starts the <see cref="IProcess"/>.
        /// </summary>
        public void Start()
        {
            if (IsRunning)
                return;

            Events.ProcessSetup?.Invoke(this, new ProcessEventArgs(currentProcess));

            if (ServiceRegistry.Has<ModeService>())
                ServiceRegistry.Get<ModeService>().ModeHandler.ModeChanged += HandleModeChanged;

            currentProcess.LifeCycle.StageChanged += HandleProcessStageChanged;
            currentProcess.Configure(ServiceRegistry.Get<IModeService>());

            var stepLockService = ServiceRegistry.Get<IStepLockService>();
            stepLockService?.Configure(ServiceRegistry.Get<IModeService>());
            stepLockService?.OnProcessStarted(currentProcess);
            currentProcess.LifeCycle.Activate();

            Events.ProcessStarted?.Invoke(this, new ProcessEventArgs(currentProcess));
        }

        /// <summary>
        /// Sets the specified chapter as the next chapter in the process.
        /// </summary>     
        public void SetNextChapter(IChapter chapter)
        {
            CurrentProcess.Data.OverrideNext = chapter;
        }

        /// <summary>
        /// Skips the current step and uses given transition.
        /// </summary>
        /// <param name="transition">Transition which should be used.</param>
        public void SkipStep(ITransition transition)
        {
            if (IsRunning == false)
            {
                return;
            }

            CurrentProcess.Data.Current.Data.Current.LifeCycle.MarkToFastForward();
            transition.Autocomplete();

            Events.FastForwardStep?.Invoke(this, new FastForwardProcessEventArgs(transition, CurrentProcess));
        }

        /// <summary>
        /// Skips the given amount of chapters.
        /// </summary>
        /// <param name="numberOfChapters">Number of chapters.</param>
        public void SkipChapters(int numberOfChapters)
        {
            IList<IChapter> chapters = CurrentProcess.Data.Chapters;

            foreach (IChapter currentChapter in chapters.Skip(chapters.IndexOf(CurrentProcess.Data.Current)).Take(numberOfChapters))
            {
                currentChapter.LifeCycle.MarkToFastForward();
            }
        }

        /// <summary>
        /// Skips the current chapters.
        /// </summary>
        public void SkipCurrentChapter()
        {
            if (IsRunning == false)
            {
                return;
            }

            IChapter currentChapter = CurrentProcess.Data.Current;
            if (currentChapter.LifeCycle.Stage == Stage.Inactive)
            {
                currentChapter.LifeCycle.Activate();
            }

            currentChapter.LifeCycle.MarkToFastForward();
            currentChapter.LifeCycle.Deactivate();
        }

        public void OnSceneUnloaded(string sceneName)
        {
            events = null;
        }

        public void Stop()
        {
        }

        private void HandleModeChanged(object sender, ModeChangedEventArgs args)
        {
            if (currentProcess != null)
            {
                currentProcess.Configure(args.ModeService);
                ServiceRegistry.Get<IStepLockService>()?.Configure(ServiceRegistry.Get<IModeService>());
            }
        }

        private void HandleProcessStageChanged(object sender, ActivationStateChangedEventArgs e)
        {
            if (e.Stage == Stage.Inactive)
            {
                if (ServiceRegistry.Has<ModeService>())
                    ServiceRegistry.Get<ModeService>().ModeHandler.ModeChanged -= HandleModeChanged;
                Stop();
            }
        }

        public void Initialize()
        {
        }
    }
}