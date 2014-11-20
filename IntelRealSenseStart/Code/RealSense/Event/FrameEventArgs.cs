using IntelRealSenseStart.Code.RealSense.Data;

namespace IntelRealSenseStart.Code.RealSense.Component.Event
{
    public class FrameEventArgs
    {
        private HandsData handsData;
        private ImageData imageData;

        public class Builder
        {
            private readonly FrameEventArgs frameEventArgs;

            public Builder()
            {
                frameEventArgs = new FrameEventArgs();
            }

            public Builder WithImageData(ImageData.Builder imageData)
            {
                frameEventArgs.imageData = imageData.Build();
                return this;
            }

            public Builder WithHandsData(HandsData.Builder handsData)
            {
                frameEventArgs.handsData = handsData.Build();
                return this;
            }

            public FrameEventArgs Build()
            {
                return frameEventArgs;
            }
        }
    }
}