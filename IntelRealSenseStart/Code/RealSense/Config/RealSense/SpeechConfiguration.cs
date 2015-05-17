namespace IntelRealSenseStart.Code.RealSense.Config.RealSense
{
    public class SpeechConfiguration
    {
        private const float DEFAULT_VOLUME = 0.2f;

        private bool usingDictation;
        private float volume;

        public bool UsingDictation
        {
            get { return usingDictation; }
        }

        public float Volume
        {
            get { return volume; }
        }

        public class Builder
        {
            private readonly SpeechConfiguration configuration;

            public Builder()
            {
                configuration = new SpeechConfiguration();
                WithDefaultValues();
            }

            public Builder WithDefaultValues()
            {
                return UsingDictation().WithVolume(DEFAULT_VOLUME);
            }

            private Builder WithVolume(float volume)
            {
                configuration.volume = volume;
                return this;
            }

            public Builder UsingDictation()
            {
                configuration.usingDictation = true;
                return this;
            }

            public SpeechConfiguration Build()
            {
                return configuration;
            }
        }
    }
}