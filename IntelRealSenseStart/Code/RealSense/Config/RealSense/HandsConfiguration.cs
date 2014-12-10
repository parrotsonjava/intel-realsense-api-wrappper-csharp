namespace IntelRealSenseStart.Code.RealSense.Config.RealSense
{
    public class HandsConfiguration
    {
        private bool segmentationImageEnabled;

        private HandsConfiguration()
        {
            segmentationImageEnabled = false;
        }

        public bool SegmentationImageEnabled
        {
            get { return segmentationImageEnabled; }
        }

        public class Builder
        {
            private readonly HandsConfiguration configuration;

            public Builder()
            {
                configuration = new HandsConfiguration();
            }

            public Builder WithSegmentationImage()
            {
                configuration.segmentationImageEnabled = true;
                return this;
            }

            public HandsConfiguration Build()
            {
                return configuration;
            }
        }
    }
}