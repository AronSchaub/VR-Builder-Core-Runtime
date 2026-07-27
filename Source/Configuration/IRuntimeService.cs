using System.Threading.Tasks;
using VRBuilder.Core.Registry;
using VRBuilder.Core.Utils.Logging;

namespace VRBuilder.Core.Configuration
{
    public interface IRuntimeService : IService<IRuntimeServiceConfiguration>
    {
        string? SelectedProcess { get; set; }
        string? ManifestFileName { get; set; }
        string SelectedProcessStreamingAssetsPath { get; set; }
        IRuntimeConfigurator Configurator { get; set; }
        ILifeCycleLoggingConfiguration LifeCycleLogging { get; }
        public Task<IProcess> LoadProcess(string path);
    }
}