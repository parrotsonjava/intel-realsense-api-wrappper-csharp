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

        private PXCMCapture.Device device;

        private ImageDeterminerComponent(RealSenseFactory factory, PXCMSenseManager manager, RealSenseConfiguration configuration)
        {
            this.factory = factory;
            this.manager = manager;
            this.configuration = configuration;
        }

        public void EnableFeatures()
        {
            if (configuration.Image.ColorEnabled)
            {
                StreamConfiguration streamConfiguration = configuration.Image.ColorStreamConfiguration;
                manager.EnableStream(PXCMCapture.StreamType.STREAM_TYPE_COLOR, streamConfiguration.Resolution.Width,
                    streamConfiguration.Resolution.Height, streamConfiguration.FrameRate);
            }
            if (configuration.Image.DepthEnabled || configuration.HandsDetectionEnabled)
            {
                StreamConfiguration streamConfiguration = configuration.Image.DepthStreamConfiguration;
                manager.EnableStream(PXCMCapture.StreamType.STREAM_TYPE_DEPTH, streamConfiguration.Resolution.Width,
                    streamConfiguration.Resolution.Height, streamConfiguration.FrameRate);
            }
        }

        public void Configure()
        {
            device = manager.QueryCaptureManager().QueryDevice();
        }

        public bool ShouldBeStarted
        {
            get { return configuration.Image.ColorEnabled || configuration.Image.DepthEnabled; }
        }

        public void Process(DeterminerData.Builder determinerData)
        {
            PXCMCapture.Sample realSenseSample = manager.QuerySample();

            determinerData.WithImageData(
                factory.Data.Determiner.Image()
                    .WithColorImage(realSenseSample.color)
                    .WithDepthImage(realSenseSample.depth)
                    .WithUvMap(CreateUvMapFrom(realSenseSample)));
        }

        private PXCMPointF32[] CreateUvMapFrom(PXCMCapture.Sample realSenseSample)
        {
            if (!configuration.Image.ProjectionEnabled)
            {
                return null;
            }

            var projection = device.CreateProjection();
            var uvMap = new PXCMPointF32[realSenseSample.depth.info.width * realSenseSample.depth.info.height];
            projection.QueryUVMap(realSenseSample.depth, uvMap);
            projection.Dispose();

            return uvMap;
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
                factory.Check(Preconditions.IsNotNull,
                    "The factory must be set in order to create the hands determiner component");
                manager.Check(Preconditions.IsNotNull,
                    "The RealSense manager must be set in order to create the hands determiner component");
                configuration.Check(Preconditions.IsNotNull,
                    "The RealSense configuration must be set in order to create the hands determiner component");

                return new ImageDeterminerComponent(factory, manager, configuration);
            }
        }
    }
}