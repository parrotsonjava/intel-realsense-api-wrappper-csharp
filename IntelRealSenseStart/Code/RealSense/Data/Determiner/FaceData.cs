namespace IntelRealSenseStart.Code.RealSense.Data.Determiner
{
    public class FaceData
    {
        private PXCMFaceData.LandmarkPoint[] landmarkPoints;

        public PXCMFaceData.LandmarkPoint[] LandmarkPoints
        {
            get { return landmarkPoints; }
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
        }
    }
}