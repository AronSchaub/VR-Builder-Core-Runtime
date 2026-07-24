// Modifications copyright (c) 2026 Aron Schaub
// SPDX-License-Identifier: Apache-2.0

using VRBuilder.Core.Localization;

namespace VRBuilder.Core.TextToSpeech
{
    /// <summary>
    /// Utility implementation of the <see cref="ITextToSpeechContent"/> interface that provides a default <see cref="IsCached"/> method.
    /// </summary>
    public abstract class TextToSpeechContent : ITextToSpeechContent, ILocalizedContent
    {
        /// <inheritdoc/>
        public abstract string Text { get; set; }
        
        /// <inheritdoc/>
        public abstract string Speaker { get; set; }

        /// <inheritdoc/>
        public abstract string GetLocalizedContent();
    }
}
