namespace IntelRealSenseStart.Code.RealSense.Data.Properties
{
    public class RealSenseProperties
    {
        private VideoProperties videoProperties;
        private AudioProperties audioProperties; 

        private RealSenseProperties()
        {
        }

        public VideoProperties Video
        {
            get { return videoProperties; }
        }

        public AudioProperties Audio
        {
            get { return audioProperties; }
        }

        public class Builder
        {
            private readonly RealSenseProperties realSenseProperties;
            
            public Builder()
            {
                realSenseProperties = new RealSenseProperties();   
            }

            public Builder WithVideoProperties(VideoProperties.Builder videoProperties)
            {
                realSenseProperties.videoProperties = videoProperties.Build();
                return this;
            }

            public Builder WithAudioProperties(AudioProperties.Builder audioProperties)
            {
                realSenseProperties.audioProperties = audioProperties.Build();
                return this;
            }

            public RealSenseProperties Build()
            {
                return realSenseProperties;
            }
        }
    }
}