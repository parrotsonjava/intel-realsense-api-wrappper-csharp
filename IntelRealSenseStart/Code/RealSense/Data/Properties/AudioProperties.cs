using System.Collections.Generic;

namespace IntelRealSenseStart.Code.RealSense.Data.Properties
{
    public class AudioProperties
    {
        private readonly List<AudioDeviceProperties> audioDeviceProperties;
        private readonly List<AudioModuleProperties> audioModuleProperties;

        private AudioProperties()
        {
            audioDeviceProperties = new List<AudioDeviceProperties>();
            audioModuleProperties = new List<AudioModuleProperties>();
        }

        public List<AudioDeviceProperties> Devices
        {
            get { return audioDeviceProperties; }
        }

        public List<AudioModuleProperties> Modules
        {
            get { return audioModuleProperties; }
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

            public Builder WithModule(AudioModuleProperties.Builder audioModule)
            {
                audioProperties.audioModuleProperties.Add(audioModule.Build());
                return this;
            }

            public AudioProperties Build()
            {
                return audioProperties;
            }
        }
    }
}