using VRBuilder.Core.ProcessRunning;
using VRBuilder.Core.SceneObjects;
using VRBuilder.Core.Utils.Audio;

namespace VRBuilder.Core.User
{
    public interface IUserService : IVRBService
    {
        IUserSceneObject User { get; set; }
        IAudioData InstructionPlayer { get; }
    }
}