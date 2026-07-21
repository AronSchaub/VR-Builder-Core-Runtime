using System;
using VRBuilder.Core.SceneObjects;

namespace VRBuilder.Core.Multiplayer
{
    public interface IMultiplayerService
    {
        /// <summary>
        /// Requests authority on the specified scene object.
        /// </summary>
        public void RequestAuthority(ISceneObject sceneObject, Action<ISceneObject> onAuthorityGranted = null);
    }
}