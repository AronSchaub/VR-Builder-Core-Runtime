using System;
using System.Threading.Tasks;
using VRBuilder.Core.ProcessRunning;
using VRBuilder.Core.Registry;
using VRBuilder.Core.Utils.Logging;

namespace VRBuilder.Core.Configuration
{
    /// <summary>
    /// Provides access to the runtime configuration and process handling (load and start) for the current application session.
    /// </summary>
    public interface IRuntimeService : IService<IRuntimeServiceConfiguration>
    {
        /// <summary>
        /// Raised when the selected process changes.
        /// </summary>
        public event Action<string?> SelectedProcessChanged;
        
        /// <summary>
        /// The name of the selected process, or <c>null</c> if none is selected.
        /// </summary>
        string? SelectedProcess { get; set; }
        
        /// <summary>
        /// Process handler who handels the process runner at engine runtime.
        /// </summary>
        IConfigurableProcessHandler ProcessHandler { get; set; }

        /// <summary>
        /// The file name of the process manifest, or <c>null</c> if none is used.
        /// </summary>
        string? ManifestFileName { get; set; }

        /// <summary>
        /// The streaming-assets path of the selected process.
        /// </summary>
        string SelectedProcessStreamingAssetsPath { get; set; }

        /// <summary>
        /// The runtime configurator of the current session.
        /// </summary>
        IRuntimeConfigurator Configurator { get; set; }

        /// <summary>
        /// Configuration of which lifecycle events should be logged.
        /// </summary>
        ILifeCycleLoggingConfiguration LifeCycleLogging { get; }

        /// <summary>
        /// Loads the process from the given path.
        /// </summary>
        /// <param name="path">The path to the process file.</param>
        /// <returns>The loaded process.</returns>
        Task<IProcess> LoadProcess(string path = "");
        
        /// <summary>
        /// Initializes the process for an existing process handler.
        /// </summary>
        /// <param name="process">The process to run.</param>
        void LoadProcess(IProcess process);
        
        /// <summary>
        /// Starts the loaded process.
        /// </summary>
        void StartProcess();
    }
}