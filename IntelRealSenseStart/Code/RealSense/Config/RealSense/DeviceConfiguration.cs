namespace IntelRealSenseStart.Code.RealSense.Config.RealSense
{
    public class DeviceConfiguration
    {
        public static readonly DeviceConfiguration DEFAULT_CONFIGURATION;

        private VideoDeviceConfiguration videoDeviceConfiguration;
        private AudioDeviceConfiguration audioDeviceConfiguration;
        
        static DeviceConfiguration()
        {
            DEFAULT_CONFIGURATION = new DeviceConfiguration();
        }

        private DeviceConfiguration()
        {
            videoDeviceConfiguration = VideoDeviceConfiguration.DEFAULT_CONFIGURATION;
            audioDeviceConfiguration = AudioDeviceConfiguration.DEFAULT_CONFIGURATION;
        }

        public VideoDeviceConfiguration VideoDevice
        {
            get { return videoDeviceConfiguration; }
        }

        public AudioDeviceConfiguration AudioDevice
        {
            get { return audioDeviceConfiguration; }
        }

        public class Builder
        {
            private readonly DeviceConfiguration deviceConfiguration;

            public Builder()
            {
                deviceConfiguration = new DeviceConfiguration();
            }

            public Builder WithVideoDeviceConfiguration(VideoDeviceConfiguration.Builder videoDeviceConfiguration)
            {
                deviceConfiguration.videoDeviceConfiguration = videoDeviceConfiguration.Build();
                return this;
            }

            public Builder WithAudioDeviceConfiguration(AudioDeviceConfiguration.Builder audioDeviceConfiguration)
            {
                deviceConfiguration.audioDeviceConfiguration = audioDeviceConfiguration.Build();
                return this;
            }

            public DeviceConfiguration Build()
            {
                return deviceConfiguration;
            }
        }
    }
}