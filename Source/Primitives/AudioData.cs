// Copyright (c) 2026 Aron Schaub
// SPDX-License-Identifier: Apache-2.0

using System.Runtime.Serialization;
using VRBuilder.Core.Utils.Audio;

namespace VRBuilder.Core.Primitives
{
    [DataContract]
    public class AudioData : IAudioData
    {
        private readonly string clipName;
        private readonly bool hasAudio;
        private bool isLoading;
        private bool isReady;
        private string clipData;
        private readonly IAudioClip audioClip;

        public bool HasAudio => audioClip is { RawAudioData: { Length: > 0 } };

        public bool IsLoading => isLoading;

        public bool IsReady => isReady;

        public string ClipData
        {
            get => clipData;
            set => clipData = value;
        }

        public IAudioClip AudioClip => audioClip;

        public AudioData()
        {
            audioClip = new AudioClipData(new byte[] { }, 0, 0);
            isReady = true;
            isLoading = false;
        }

        public AudioData(AudioClipData audioClip, string clipName)
        {
            this.audioClip = audioClip;
            this.clipName = clipName;
            isReady = false;
            isLoading = true;
        }

        public void Initialize()
        {
            isReady = true;
            isLoading = false;
        }

        public bool IsEmpty()
        {
            return !HasAudio;
        }
    }
}