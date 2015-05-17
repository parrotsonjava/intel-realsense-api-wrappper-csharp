namespace IntelRealSenseStart.Code.RealSense.Config.RealSense
{
    public class BaseConfiguration
    {
        public static readonly BaseConfiguration DEFAULT_CONFIGURATION;

        private VideoConfiguration videoConfiguration;
        private AudioConfiguration audioConfiguration;
        
        static BaseConfiguration()
        {
            DEFAULT_CONFIGURATION = new BaseConfiguration();
        }

        private BaseConfiguration()
        {
            videoConfiguration = VideoConfiguration.DEFAULT_CONFIGURATION;
            audioConfiguration = AudioConfiguration.DEFAULT_CONFIGURATION;
        }

        public VideoConfiguration Video
        {
            get { return videoConfiguration; }
        }

        public AudioConfiguration Audio
        {
            get { return audioConfiguration; }
        }

        public class Builder
        {
            private readonly BaseConfiguration baseConfiguration;

            public Builder()
            {
                baseConfiguration = new BaseConfiguration();
            }

            public Builder WithVideoConfiguration(VideoConfiguration.Builder videoConfiguration)
            {
                baseConfiguration.videoConfiguration = videoConfiguration.Build();
                return this;
            }

            public Builder WithAudioConfiguration(AudioConfiguration.Builder audioConfiguration)
            {
                baseConfiguration.audioConfiguration = audioConfiguration.Build();
                return this;
            }

            public BaseConfiguration Build()
            {
                return baseConfiguration;
            }
        }
    }
}