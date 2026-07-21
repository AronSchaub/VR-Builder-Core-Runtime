namespace VRBuilder.Core.Input
{
    public static class InputLocator
    {
        private static IInputController? current;
        public static IInputController? Current { get; set; }

        [System.Diagnostics.CodeAnalysis.MemberNotNullWhen(true, nameof(Current))]
        public static bool IsRegistered => current != null;
    }
}