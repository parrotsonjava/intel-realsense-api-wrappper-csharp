namespace IntelRealSenseStart.Code.RealSense.Config.RealSense
{
    public class VideoDeviceConfiguration
    {
        public static readonly VideoDeviceConfiguration DEFAULT_CONFIGURATION;

        static VideoDeviceConfiguration()
        {
            DEFAULT_CONFIGURATION = new VideoDeviceConfiguration();
        }

        private VideoDeviceConfiguration()
        {
        }

        public class Builder
        {
            private readonly VideoDeviceConfiguration videoDeviceConfiguration;

            public Builder()
            {
                videoDeviceConfiguration = new VideoDeviceConfiguration();
            }

            public VideoDeviceConfiguration Build()
            {
                return videoDeviceConfiguration;
            }
        }
    }
}