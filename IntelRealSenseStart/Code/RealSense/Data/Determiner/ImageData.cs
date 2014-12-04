namespace IntelRealSenseStart.Code.RealSense.Data
{
    public class ImageData
    {
        private PXCMImage colorImage;
        private PXCMImage depthImage;

        private ImageData()
        {
            colorImage = null;
            depthImage = null;
        }

        public PXCMImage ColorImage
        {
            get { return colorImage; }
        }

        public bool HasColorImage
        {
            get { return colorImage != null; }
        }

        public PXCMImage DepthImage
        {
            get { return depthImage; }
        }

        public bool HasDepthImage
        {
            get { return depthImage != null; }
        }

        public class Builder
        {
            private readonly ImageData imageData;

            public Builder()
            {
                imageData = new ImageData();
            }

            public Builder WithColorImage(PXCMImage colorImage)
            {
                imageData.colorImage = colorImage;
                return this;
            }

            public Builder WithDepthImage(PXCMImage depthImage)
            {
                imageData.depthImage = depthImage;
                return this;
            }

            public ImageData Build()
            {
                return imageData;
            }
        }
    }
}