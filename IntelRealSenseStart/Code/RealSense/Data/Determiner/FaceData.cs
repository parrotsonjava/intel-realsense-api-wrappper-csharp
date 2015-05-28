namespace IntelRealSenseStart.Code.RealSense.Data.Determiner
{
    public class FaceData
    {
        private PXCMFaceData.LandmarkPoint[] landmarkPoints;

        private PXCMFaceData.PulseData pulseData;

        private int faceId;
        private int recognizedId;

        public PXCMFaceData.LandmarkPoint[] LandmarkPoints
        {
            get { return landmarkPoints; }
        }

        public PXCMFaceData.PulseData PulseData
        {
            get { return pulseData; }
        }

        public int FaceId
        {
            get { return faceId; }
        }

        public int RecognizedId
        {
            get { return recognizedId; }
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

            public Builder WithFaceId(int userId)
            {
                faceData.faceId = userId;
                return this;
            }

            public Builder WithRecognizedId(int recognizedId)
            {
                faceData.recognizedId = recognizedId;
                return this;
            }
        }
    }
}