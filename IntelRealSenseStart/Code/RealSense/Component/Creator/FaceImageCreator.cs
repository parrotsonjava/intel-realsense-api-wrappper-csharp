using System.Drawing;
using IntelRealSenseStart.Code.RealSense.Config.Image;
using IntelRealSenseStart.Code.RealSense.Config.RealSense;
using IntelRealSenseStart.Code.RealSense.Data.Determiner;

namespace IntelRealSenseStart.Code.RealSense.Component.Creator
{
    public class FaceImageCreator : ImageCreator
    {
        // ReSharper disable once NotAccessedField.Local
        private readonly RealSenseConfiguration realSenseConfiguration;

        private FaceImageCreator(RealSenseConfiguration realSenseConfiguration)
        {
            this.realSenseConfiguration = realSenseConfiguration;
        }

        public Bitmap Create(Bitmap bitmap, DeterminerData determinerData,
            ImageCreatorConfiguration imageCreatorConfiguration)
        {
            return bitmap;
        }

        public class Builder
        {
            private RealSenseConfiguration realSenseConfiguration;

            public Builder WithRealSenseConfiguration(RealSenseConfiguration realSenseConfiguration)
            {
                this.realSenseConfiguration = realSenseConfiguration;
                return this;
            }

            public FaceImageCreator Build()
            {
                return new FaceImageCreator(realSenseConfiguration);
            }
        }
    }
}