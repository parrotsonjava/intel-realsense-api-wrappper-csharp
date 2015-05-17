namespace IntelRealSenseStart.Code.RealSense.Config.RealSense
{
    public class SpeechSynthesisConfiguration
    {
        private int volume;
        private float speechRate;
        private int pitch;

        private SpeechSynthesisConfiguration()
        {
        }

        public int Volume
        {
            get { return volume; }
        }

        public float SpeechRate
        {
            get { return speechRate; }
        }

        public int Pitch
        {
            get { return pitch; }
        }

        public class Builder
        {
            private readonly SpeechSynthesisConfiguration configuration;

            public Builder()
            {
                configuration = new SpeechSynthesisConfiguration();
                WithDefaultValues();
            }

            public Builder WithDefaultValues()
            {
                return WithVolume(80).WithSpeechRate(100).WithPitch(100);
            }

            public Builder WithVolume(int volume)
            {
                configuration.volume = volume;
                return this;
            }

            public Builder WithSpeechRate(float speechRate)
            {
                configuration.speechRate = speechRate;
                return this;
            }

            public Builder WithPitch(int pitch)
            {
                configuration.pitch = pitch;
                return this;
            }

            public SpeechSynthesisConfiguration Build()
            {
                return configuration;
            }
        }
    }
}