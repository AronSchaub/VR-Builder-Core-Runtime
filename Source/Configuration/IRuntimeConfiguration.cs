namespace VRBuilder.Core.Configuration
{
    public interface IRuntimeConfiguration
    {
        string? SelectedProcess { get; set; }
        string ManifestFileName { get; set; }
    }
}