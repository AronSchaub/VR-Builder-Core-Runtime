// Modifications copyright (c) 2026 Aron Schaub
// SPDX-License-Identifier: Apache-2.0

using VRBuilder.Core.Registry;
using VRBuilder.Core.TextToSpeech.Providers;
using VRBuilder.Core.TextToSpeech.Utils;

namespace Source.TextToSpeech
{
	public interface ITextToSpeechService : IService<ITextToSpeechConfiguration> 
	{
		public ITextToSpeechProvider DefaultOrActiveTextToSpeechProvider { get; set; }
		
		public ITextToSpeechConfiguration Configuration { get; set; }

		/// <summary>
		/// Get a full path based on a <paramref name="fileLocator"/> information to produce speech from, and create a directory for that.
		/// </summary>
		/// <param name="fileLocator">Locator information of the text-to-speech file.</param>
		/// <returns>True if the localizedContent in the chosen locale is cached.</returns>
		public string PrepareFilepathForTextToSpeechFile(ITextToSpeechFileLocator fileLocator);
	}
}