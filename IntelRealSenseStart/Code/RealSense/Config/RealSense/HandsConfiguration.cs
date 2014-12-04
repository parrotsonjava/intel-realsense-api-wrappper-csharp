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
            private readonly HandsConfiguration handsFeature;

            public Builder()
            {
                handsFeature = new HandsConfiguration();
            }

            public Builder WithSegmentationImage()
            {
                handsFeature.segmentationImageEnabled = true;
                return this;
            }

            public HandsConfiguration Build()
            {
                return handsFeature;
            }
        }
    }
}