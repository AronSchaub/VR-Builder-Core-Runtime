namespace VRBuilder.Core.Configuration.Modes
{
	/// <summary>
	/// Simple service locator for <see cref="IModeService"/>.
	/// Allows engine-agnostic code to access the process runner without a direct dependency on Unity.
	/// The Unity-side registers itself via <c>[RuntimeInitializeOnLoadMethod]</c>.
	/// </summary>
	public class ModeLocator
	{
		private static IModeService? current;

		/// <summary>
		/// The current <see cref="IModeService"/> instance.
		/// </summary>
		public static IModeService? Current
		{
			get => current;
			set => current = value;
		}
		
		/// <summary>
		/// True if a process runner has been registered.
		/// </summary>
		public static bool IsRegistered => current != null;
	}
}