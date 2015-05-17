using System.Collections.Generic;

namespace IntelRealSenseStart.Code.RealSense.Data.Properties
{
    public class AudioProperties
    {
        private readonly List<AudioDeviceProperties> audioDeviceProperties;

        private AudioProperties()
        {
            audioDeviceProperties = new List<AudioDeviceProperties>();
        }

        public List<AudioDeviceProperties> Devices
        {
            get { return audioDeviceProperties; }
        }

        public class Builder
        {
            private readonly AudioProperties audioProperties;

            public Builder()
            {
                audioProperties = new AudioProperties();
            }

            public Builder WithAudioDevice(AudioDeviceProperties.Builder audioDeviceProperties)
            {
                audioProperties.audioDeviceProperties.Add(audioDeviceProperties.Build());
                return this;
            }

            public AudioProperties Build()
            {
                return audioProperties;
            }
        }
    }
}