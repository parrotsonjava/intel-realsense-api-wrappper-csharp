namespace IntelRealSenseStart.Code.RealSense.Data.Determiner
{
    public class FaceData
    {
        private PXCMFaceData.LandmarkPoint[] landmarkPoints;
        private PXCMFaceData.PulseData pulseData;

        public PXCMFaceData.LandmarkPoint[] LandmarkPoints
        {
            get { return landmarkPoints; }
        }

        public PXCMFaceData.PulseData PulseData
        {
            get { return pulseData; }
        }

        public class Builder
        {
            private readonly FaceData faceData;

            public Builder()
            {
                faceData = new FaceData();
            }

            public FaceData Build()
            {
                return faceData;
            }

            public Builder WithLandmarks(PXCMFaceData.LandmarkPoint[] landmarkPoints)
            {
                faceData.landmarkPoints = landmarkPoints;
                return this;
            }

            public Builder WithPulse(PXCMFaceData.PulseData pulseData)
            {
                faceData.pulseData = pulseData;
                return this;
            }
        }
    }
}