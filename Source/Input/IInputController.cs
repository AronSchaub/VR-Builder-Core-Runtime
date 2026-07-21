using VRBuilder.Core.ProcessRunning;

namespace VRBuilder.Core.Input
{
    public interface IInputController : IVRBService
    {
        void SetupInputActions();
        void LoadInputActions();
        bool UsesCustomKeyBindingAsset();
    }
}