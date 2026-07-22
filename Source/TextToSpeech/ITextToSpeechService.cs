using VRBuilder.Core.Registry;
using VRBuilder.Core.TextToSpeech.Providers;

namespace Source.TextToSpeech
{
	public interface ITextToSpeechService : IService<ITextToSpeechConfiguration> 
	{
		public ITextToSpeechProvider DefaultOrActiveTextToSpeechProvider { get; set; }
		
		public ITextToSpeechConfiguration Configuration
		{
			get;
			set;
		}
	}
}