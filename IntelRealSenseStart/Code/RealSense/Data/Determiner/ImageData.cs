namespace IntelRealSenseStart.Code.RealSense.Data.Determiner
{
    public class ImageData
    {
        private PXCMImage colorImage;
        private PXCMImage depthImage;

        private PXCMPointF32[] uvMap;

        private ImageData()
        {
            colorImage = null;
            depthImage = null;
            uvMap = null;
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

        public PXCMPointF32[] UVMap
        {
            get { return uvMap; }
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

            public Builder WithUvMap(PXCMPointF32[] uvMap)
            {
                imageData.uvMap = uvMap;
                return this;
            }

            public ImageData Build()
            {
                return imageData;
            }
        }
    }
}