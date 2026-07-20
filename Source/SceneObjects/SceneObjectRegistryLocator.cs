namespace VRBuilder.Core.SceneObjects
{
    public class SceneObjectRegistryLocator
    {
        private static ISceneObjectRegistry? current;

        /// <summary>
        /// The current <see cref="ISceneObjectRegistry"/> instance.
        /// </summary>
        public static ISceneObjectRegistry? Current
        {
            get => current;
            set => current = value;
        }

        /// <summary>
        /// True if a process runner has been registered.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.MemberNotNullWhen(true, nameof(Current))]
        public static bool IsRegistered => current != null;
    }
}