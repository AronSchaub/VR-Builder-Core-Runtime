using System.Threading.Tasks;
using VRBuilder.Core.Registry;

namespace VRBuilder.Core.Configuration
{
    public interface IRuntimeService : IService<IRuntimeConfiguration>
    {
        string SelectedProcess { get; set; }
        string ManifestFileName { get; set; }
        string SelectedProcessStreamingAssetsPath { get; set; }
        IRuntimeConfigurator Configurator { get; set; }
        public Task<IProcess> LoadProcess(string path);
    }
}