// Modifications copyright (c) 2026 Aron Schaub
// SPDX-License-Identifier: Apache-2.0

using System;

namespace VRBuilder.Core.TextToSpeech
{
    public class UnableToParseAudioFormatException : Exception
    {
        public UnableToParseAudioFormatException(string msg) : base(msg) { }
    }
}
