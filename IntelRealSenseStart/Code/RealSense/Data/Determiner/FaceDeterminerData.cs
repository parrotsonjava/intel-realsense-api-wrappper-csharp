namespace IntelRealSenseStart.Code.RealSense.Data.Determiner
{
    public class FaceDeterminerData
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
            private readonly FaceDeterminerData faceDeterminerData;

            public Builder()
            {
                faceDeterminerData = new FaceDeterminerData();
            }

            public FaceDeterminerData Build()
            {
                return faceDeterminerData;
            }

            public Builder WithLandmarks(PXCMFaceData.LandmarkPoint[] landmarkPoints)
            {
                faceDeterminerData.landmarkPoints = landmarkPoints;
                return this;
            }

            public Builder WithPulse(PXCMFaceData.PulseData pulseData)
            {
                faceDeterminerData.pulseData = pulseData;
                return this;
            }

            public Builder WithFaceId(int userId)
            {
                faceDeterminerData.faceId = userId;
                return this;
            }

            public Builder WithRecognizedId(int recognizedId)
            {
                faceDeterminerData.recognizedId = recognizedId;
                return this;
            }
        }
    }
}