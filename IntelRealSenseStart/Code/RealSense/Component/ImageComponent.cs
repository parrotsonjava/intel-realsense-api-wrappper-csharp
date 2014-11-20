using IntelRealSenseStart.Code.RealSense.Config;
using IntelRealSenseStart.Code.RealSense.Event;
using IntelRealSenseStart.Code.RealSense.Factory;

namespace IntelRealSenseStart.Code.RealSense.Component
{
    public class ImageComponent : Component
    {
        private readonly Configuration configuration;

        private readonly RealSenseFactory factory;
        private readonly PXCMSenseManager manager;

        private ImageComponent(RealSenseFactory factory, PXCMSenseManager manager, Configuration configuration)
        {
            this.factory = factory;
            this.manager = manager;
            this.configuration = configuration;
        }

        public void EnableFeatures()
        {
            if (configuration.ColorImageEnabled)
            {
                manager.EnableStream(PXCMCapture.StreamType.STREAM_TYPE_COLOR, configuration.ColorImage.Resolution.Width,
                    configuration.ColorImage.Resolution.Height);
            }
            if (configuration.DepthImageEnabled)
            {
                manager.EnableStream(PXCMCapture.StreamType.STREAM_TYPE_DEPTH, configuration.DepthImage.Resolution.Width,
                    configuration.DepthImage.Resolution.Height);
            }
        }

        public void Configure()
        {
        }

        public bool ShouldBeStarted
        {
            get { return configuration.HandsDetectionEnabled; }
        }

        public void Process(FrameEventArgs.Builder frameEvent)
        {
            PXCMCapture.Sample realSenseSample = manager.QuerySample();

            frameEvent.WithImageData(
                factory.Data.ImageData()
                    .WithColorImage(realSenseSample.color)
                    .WithDepthImage(realSenseSample.depth));

        }

        public class Builder
        {
            public ImageComponent Build(RealSenseFactory factory, PXCMSenseManager pxcmSenseManager,
                Configuration configuration)
            {
                return new ImageComponent(factory, pxcmSenseManager, configuration);
            }
        }
    }
}