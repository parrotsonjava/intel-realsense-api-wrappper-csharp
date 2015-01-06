using IntelRealSenseStart.Code.RealSense.Config.RealSense;
using IntelRealSenseStart.Code.RealSense.Data.Determiner;
using IntelRealSenseStart.Code.RealSense.Factory;
using IntelRealSenseStart.Code.RealSense.Helper;

namespace IntelRealSenseStart.Code.RealSense.Component.Determiner
{
    public class ImageDeterminerComponent : DeterminerComponent
    {
        private readonly RealSenseConfiguration configuration;

        private readonly RealSenseFactory factory;
        private readonly PXCMSenseManager manager;

        private ImageDeterminerComponent(RealSenseFactory factory, PXCMSenseManager manager, RealSenseConfiguration configuration)
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
            if (configuration.DepthImageEnabled || configuration.HandsDetectionEnabled)
            {
                manager.EnableStream(PXCMCapture.StreamType.STREAM_TYPE_DEPTH, configuration.DepthImage.Resolution.Width,
                    configuration.DepthImage.Resolution.Height);
            }
        }

        public void Configure()
        {
            // Nothing to do
        }

        public bool ShouldBeStarted
        {
            get { return configuration.HandsDetectionEnabled; }
        }

        public void Process(DeterminerData.Builder determinerData)
        {
            PXCMCapture.Sample realSenseSample = manager.QuerySample();

            determinerData.WithImageData(
                factory.Data.Determiner.Image()
                    .WithColorImage(realSenseSample.color)
                    .WithDepthImage(realSenseSample.depth));
        }

        public class Builder
        {
            private RealSenseFactory factory;
            private PXCMSenseManager manager;
            private RealSenseConfiguration configuration;

            public Builder WithFactory(RealSenseFactory factory)
            {
                this.factory = factory;
                return this;
            }

            public Builder WithManager(PXCMSenseManager manager)
            {
                this.manager = manager;
                return this;
            }

            public Builder WithConfiguration(RealSenseConfiguration configuration)
            {
                this.configuration = configuration;
                return this;
            }

            public ImageDeterminerComponent Build()
            {
                factory.CheckState(Preconditions.IsNotNull,
                    "The factory must be set in order to create the hands determiner component");
                manager.CheckState(Preconditions.IsNotNull,
                    "The RealSense manager must be set in order to create the hands determiner component");
                configuration.CheckState(Preconditions.IsNotNull,
                    "The RealSense configuration must be set in order to create the hands determiner component");

                return new ImageDeterminerComponent(factory, manager, configuration);
            }
        }
    }
}