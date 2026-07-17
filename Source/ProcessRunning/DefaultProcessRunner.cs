// Copyright (c) 2026 Aron Schaub
// SPDX-License-Identifier: Apache-2.0

using System;
using System.Collections.Generic;
using System.Linq;
using VRBuilder.Core;
using VRBuilder.Core.Configuration;
using VRBuilder.Core.Configuration.Modes;
using VRBuilder.Core.ProcessRunning;

namespace VRBuilder.Unity.ProcessRunning
{
    public class DefaultProcessRunner : IProcessRunner
    {
        private IProcessRunnerConfiguration configuration;
        private ProcessEvents events;
        private IProcess currentProcess;

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

        private void HandleModeChanged(object sender, ModeChangedEventArgs args)
        {
            if (currentProcess != null)
            {
                currentProcess.Configure(args.Mode);
                RuntimeConfigurator.Configuration.StepLockHandling.Configure(RuntimeConfigurator.Configuration.Modes.CurrentMode);
            }
        }

        private void HandleProcessStageChanged(object sender, ActivationStateChangedEventArgs e)
        {
            if (e.Stage == Stage.Inactive)
            {
                RuntimeConfigurator.ModeChanged -= HandleModeChanged;
                Stop();
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
                RuntimeConfigurator.Configuration.StepLockHandling.OnProcessFinished(currentProcess);
                Events.ProcessFinished?.Invoke(this, new ProcessEventArgs(currentProcess));
            }
        }

        public void SetConfiguration(object configuration)
        {
            this.configuration = configuration as IProcessRunnerConfiguration;
        }

        public void Initialize()
        {
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

            RuntimeConfigurator.ModeChanged += HandleModeChanged;

            currentProcess.LifeCycle.StageChanged += HandleProcessStageChanged;
            currentProcess.Configure(RuntimeConfigurator.Configuration.Modes.CurrentMode);

            RuntimeConfigurator.Configuration.StepLockHandling.Configure(RuntimeConfigurator.Configuration.Modes.CurrentMode);
            RuntimeConfigurator.Configuration.StepLockHandling.OnProcessStarted(currentProcess);
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
    }
}