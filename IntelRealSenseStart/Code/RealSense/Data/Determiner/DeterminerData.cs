namespace IntelRealSenseStart.Code.RealSense.Data.Determiner
{
    public class DeterminerData
    {
        private PXCMCapture.Device device;

        private ImageData imageData;
        private HandsDeterminerData handsData;
        private FacesData facesData;

        private DeterminerData()
        {
        }

        public PXCMCapture.Device Device
        {
            get { return device; }
        }

        public ImageData ImageData
        {
            get { return imageData; }
        }

        public HandsDeterminerData HandsData
        {
            get { return handsData; }
        }

        public FacesData FacesData
        {
            get { return facesData; }
        }

        public class Builder
        {
            private readonly DeterminerData determinerData;

            public Builder()
            {
                determinerData = new DeterminerData();
            }

            public Builder WithDevice(PXCMCapture.Device device)
            {
                determinerData.device = device;
                return this;
            }

            public Builder WithImageData(ImageData.Builder imageData)
            {
                determinerData.imageData = imageData.Build();
                return this;
            }

            public Builder WithHandsData(HandsDeterminerData.Builder handsData)
            {
                determinerData.handsData = handsData.Build();
                return this;
            }

            public Builder WithFacesData(FacesData.Builder facesData)
            {
                determinerData.facesData = facesData.Build();
                return this;
            }

            public DeterminerData Build()
            {
                return determinerData;
            }
        }
    }
}