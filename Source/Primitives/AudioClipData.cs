// Copyright (c) 2026 Aron Schaub
// SPDX-License-Identifier: Apache-2.0

using System.Runtime.Serialization;

namespace VRBuilder.Core.Primitives
{
    [DataContract]
    public struct AudioClipData : IAudioClip
    {
        [DataMember]
        public byte[] RawAudioData { readonly get; set; }

        [DataMember]
        public int Frequency { readonly get; set; }

        [DataMember]
        public int Channels { readonly get; set; }

        [DataMember]
        public string Name { readonly get; set; }

        public AudioClipData(byte[] rawAudioData, int frequency, int channels, string name = null)
        {
            RawAudioData = rawAudioData;
            Frequency = frequency;
            Channels = channels;
            Name = name;
        }
    }
}