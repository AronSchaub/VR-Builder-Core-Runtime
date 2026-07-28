using TinkerFlow.addons.Core.Configuration;

namespace VRBuilder.Core.Configuration
{
    public interface IRuntimeConfigurator
    {
        IRuntimeConfiguration RuntimeConfiguration { get; set; }
    }
}