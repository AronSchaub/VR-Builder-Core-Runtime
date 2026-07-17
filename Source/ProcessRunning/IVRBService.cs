namespace VRBuilder.Core.ProcessRunning
{
    public interface IVRBService
    {
        void SetConfiguration(object configuration);
        void Initialize();
    }
}