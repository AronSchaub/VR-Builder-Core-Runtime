using VRBuilder.Core.Registry;

namespace VRBuilder.Core.Configuration
{
    public interface IRuntimeConfiguration: IServiceConfiguration
    {
        public string SelectedProcessStreamingAssetsPath { get; set; }
        string SelectedProcess { get; set; }
        string ManifestFileName { get; set; }
    }
}