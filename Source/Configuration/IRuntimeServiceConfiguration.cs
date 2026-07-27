using VRBuilder.Core.Registry;

namespace VRBuilder.Core.Configuration
{
    public interface IRuntimeServiceConfiguration: IServiceConfiguration
    {
        public string SelectedProcessStreamingAssetsPath { get; set; }
    }
}