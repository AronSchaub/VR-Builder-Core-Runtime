namespace VRBuilder.Core.User
{
    public static class UserLocator
    {
        private static IUserService? current;
        public static IUserService? Current { get; set; }

        [System.Diagnostics.CodeAnalysis.MemberNotNullWhen(true, nameof(Current))]
        public static bool IsRegistered => current != null;
    }
}