namespace IntelRealSenseStart.Code.RealSense.Config.RealSense
{
    public class FaceConfiguration
    {
        private bool useLandmarks;
        private bool useBoundingRectangle;
        private bool usePulse;

        private FaceConfiguration()
        {
            useLandmarks = false;
            usePulse = false;
        }

        public bool UseLandmarks
        {
            get { return useLandmarks;  }
        }

        public bool UseBoundingRectangle
        {
            get { return useBoundingRectangle; }
        }

        public bool UsePulse
        {
            get { return usePulse;  }
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

            public Builder UsingBoundingRectangle()
            {
                configuration.useBoundingRectangle = true;
                return this;
            }

            public Builder UsingPulse()
            {
                configuration.usePulse = true;
                return this;
            }

            public FaceConfiguration Build()
            {
                return configuration;
            }
        }
    }
}