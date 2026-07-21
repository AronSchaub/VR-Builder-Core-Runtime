namespace VRBuilder.Core.Configuration
{
    public static class SceneServiceLocator
    {
        private static ISceneService? current;
        public static ISceneService? Current { get; set; }

        [System.Diagnostics.CodeAnalysis.MemberNotNullWhen(true, nameof(Current))]
        public static bool IsRegistered => current != null;
    }
}