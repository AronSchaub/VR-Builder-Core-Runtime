using VRBuilder.Core.Registry;

namespace VRBuilder.Core.Input
{
    public interface IInputController : IService<IInputConfiguration>
    {
        void SetupInputActions();
        void LoadInputActions();
        bool UsesCustomKeyBindingAsset();
    }
}