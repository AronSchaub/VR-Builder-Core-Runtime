// Copyright (c) 2026 Aron Schaub
// SPDX-License-Identifier: Apache-2.0

namespace TinkerFlowDebug.addons.ProcessEngine.Source.Localization
{
	/// <summary>
	/// Simple service locator for <see cref="ILanguageService"/>.
	/// Allows engine-agnostic code to access the process runner without a direct dependency on Unity.
	/// The Unity-side registers itself via <c>[RuntimeInitializeOnLoadMethod]</c>.
	/// </summary>
	public static class LanguageSettingsLocator
	{
		private static ILanguageService? current;

		/// <summary>
		/// The current <see cref="ILanguageService"/> instance.
		/// </summary>
		public static ILanguageService? Current
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