using System.Drawing;
using IntelRealSenseStart.Code.RealSense.Config;
using IntelRealSenseStart.Code.RealSense.Config.HandsImage;
using IntelRealSenseStart.Code.RealSense.Data;

namespace IntelRealSenseStart.Code.RealSense.Component.Hands
{
    public class HandsImageCreator
    {
        private readonly HandsData handsData;
        private readonly ImageData imageData;

        private readonly Configuration realSenseConfiguration;
        private readonly HandsImageConfiguration imageConfiguration;

        private Bitmap bitmap = null;

        private HandsImageCreator(HandsData handsData, ImageData imageData,
            Configuration realSenseConfiguration, HandsImageConfiguration imageConfiguration)
        {
            this.handsData = handsData;
            this.imageData = imageData;

            this.realSenseConfiguration = realSenseConfiguration;
            this.imageConfiguration = imageConfiguration;
        }

        public Bitmap Create()
        {
            return null;
        }

        public class Builder
        {
            public HandsImageCreator Build(HandsData handsData, ImageData imageData,
                Configuration realSenseConfiguration, HandsImageConfiguration imageConfiguration)
            {
                return new HandsImageCreator(handsData, imageData, realSenseConfiguration, imageConfiguration);
            }
        }
    }
}