// Modifications copyright (c) 2026 Aron Schaub
// SPDX-License-Identifier: Apache-2.0

using VRBuilder.Core.Registry;
using VRBuilder.Core.SceneObjects;
using VRBuilder.Core.Utils.Audio;

namespace VRBuilder.Core.User
{
    public interface IUserService : IService<IUserConfiguration>
    {
        IUserSceneObject User { get; set; }
        IAudioData InstructionPlayer { get; }
    }
}