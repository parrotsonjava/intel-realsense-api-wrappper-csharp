namespace IntelRealSenseStart.Code.RealSense.Config.RealSense
{
    public class FaceConfiguration
    {
        private bool useLandmarks;
        private bool usePulse;

        private FaceIdentificationConfiguration identificationConfiguration;

        private FaceConfiguration()
        {
            useLandmarks = false;
            usePulse = false;
            identificationConfiguration = null;
        }

        public bool UseLandmarks
        {
            get { return useLandmarks; }
        }

        public bool UsePulse
        {
            get { return usePulse; }
        }

        public bool UseIdentification
        {
            get { return identificationConfiguration != null; }
        }

        public FaceIdentificationConfiguration Identification
        {
            get { return identificationConfiguration; }
        }

        public class Builder
        {
            private readonly FaceConfiguration configuration;

            public Builder()
            {
                configuration = new FaceConfiguration();
            }

            public Builder UsingLandmarks()
            {
                configuration.useLandmarks = true;
                return this;
            }

            public Builder UsingPulse()
            {
                configuration.usePulse = true;
                return this;
            }

            public Builder UsingFaceIdentification(
                FaceIdentificationConfiguration.Builder identificationConfiguration)
            {
                configuration.identificationConfiguration = identificationConfiguration.Build();
                return this;
            }

            public FaceConfiguration Build()
            {
                return configuration;
            }
        }
    }
}