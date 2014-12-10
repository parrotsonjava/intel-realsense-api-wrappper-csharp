namespace IntelRealSenseStart.Code.RealSense.Data.Determiner
{
    public class FaceData
    {
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
        }
    }
}
