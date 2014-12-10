namespace IntelRealSenseStart.Code.RealSense.Config.RealSense
{
    public class FaceConfiguration
    {
        private bool useLandmarks;

        private FaceConfiguration()
        {
            useLandmarks = false;
        }

        public bool UseLandmarks
        {
            get { return useLandmarks;  }
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

            public FaceConfiguration Build()
            {
                return configuration;
            }
        }
    }
}
