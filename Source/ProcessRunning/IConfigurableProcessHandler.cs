namespace VRBuilder.Core.ProcessRunning
{
    /// <summary>
    /// Interface for a process controller that can be configured in the setup object.
    /// </summary>
    public interface IConfigurableProcessHandler
    {
        /// <summary>
        /// If true, the process will start automatically as soon as the process controller is loaded.
        /// </summary>
        bool AutoStartProcess { get; set; }

        /// <summary>
        /// Starts the current selected process and handels the update of the process runner.
        /// </summary>
        void LoadAndStartProcess();
        
        /// <summary>
        /// Start the process with the current configuration.
        /// </summary>
        void StartProcess();
        
        /// <summary>
        /// Stops the active process.
        /// </summary>
        void StopProcess();

        /// <summary>
        /// Starts the process and handels the update of the process runner.
        /// </summary>
        /// <param name="process"></param>
        void Initialize(IProcess process);
    }
}