namespace IntelRealSenseStart.Code.RealSense.Config.RealSense
{
    public class FaceConfiguration
    {
        private bool useLandmarks;
        private bool usePulse;
        private bool useEmotions;

        private int maxNumberOfTrackedFaces;
        private int maxNumberOfTrackedFacesWithLandmarks;

        private FaceIdentificationConfiguration identificationConfiguration;

        private FaceConfiguration()
        {
            useLandmarks = false;
            usePulse = false;
            useEmotions = false;
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

        public bool UseEmotions
        {
            get { return useEmotions; }
        }

        public bool UseIdentification
        {
            get { return identificationConfiguration != null; }
        }

        public FaceIdentificationConfiguration Identification
        {
            get { return identificationConfiguration; }
        }

        public int MaxNumberOfTrackedFaces
        {
            get { return maxNumberOfTrackedFaces; }
        }

        public int MaxNumberOfTrackedFacesWithLandmarks
        {
            get { return maxNumberOfTrackedFacesWithLandmarks; }
        }

        public class Builder
        {
            private readonly FaceConfiguration configuration;

            public Builder()
            {
                configuration = new FaceConfiguration();
                WithDefaultConfiguration();
            }

            public Builder WithDefaultConfiguration()
            {
                return WithMaxNumberOfTrackedFaces(4)
                    .WithMaxNumberOfTrackedFacesWithLandmarks(4);
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

            public Builder UsingEmotions()
            {
                configuration.useEmotions = true;
                return this;
            }

            public Builder WithMaxNumberOfTrackedFaces(int maxNumberOfTrackedFaces)
            {
                configuration.maxNumberOfTrackedFaces = maxNumberOfTrackedFaces;
                return this;
            }

            public Builder WithMaxNumberOfTrackedFacesWithLandmarks(int maxNumberOfTrackedFacesWithLandmarks)
            {
                configuration.maxNumberOfTrackedFacesWithLandmarks = maxNumberOfTrackedFacesWithLandmarks;
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