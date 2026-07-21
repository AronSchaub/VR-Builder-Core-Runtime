using System;
using VRBuilder.Core.SceneObjects;

namespace VRBuilder.Core.Multiplayer
{
    public class NoOpMultiplayerService: IMultiplayerService
    {
        /// <inheritdoc/>
        public void RequestAuthority(ISceneObject sceneObject, Action<ISceneObject> onAuthorityGranted = null)
        {
            onAuthorityGranted?.Invoke(sceneObject);
        }
    }
}