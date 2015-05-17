using IntelRealSenseStart.Code.RealSense.Config.RealSense;
using IntelRealSenseStart.Code.RealSense.Data.Determiner;
using IntelRealSenseStart.Code.RealSense.Factory;
using IntelRealSenseStart.Code.RealSense.Helper;
using IntelRealSenseStart.Code.RealSense.Provider;

namespace IntelRealSenseStart.Code.RealSense.Component.Determiner
{
    public class ImageDeterminerComponent : FrameDeterminerComponent
    {
        private readonly RealSenseConfiguration configuration;

        private readonly RealSenseFactory factory;
        private readonly NativeSense nativeSense;

        private PXCMCapture.Device device;

        private ImageDeterminerComponent(RealSenseFactory factory, NativeSense nativeSense,
            RealSenseConfiguration configuration)
        {
            this.factory = factory;
            this.nativeSense = nativeSense;
            this.configuration = configuration;
        }

        public void EnableFeatures()
        {
            if (configuration.Image.ColorEnabled)
            {
                StreamConfiguration streamConfiguration = configuration.Image.ColorStreamConfiguration;
                nativeSense.SenseManager.EnableStream(PXCMCapture.StreamType.STREAM_TYPE_COLOR,
                    streamConfiguration.Resolution.Width, streamConfiguration.Resolution.Height,
                    streamConfiguration.FrameRate);
            }
            if (configuration.Image.DepthEnabled || configuration.HandsDetectionEnabled)
            {
                StreamConfiguration streamConfiguration = configuration.Image.DepthStreamConfiguration;
                nativeSense.SenseManager.EnableStream(PXCMCapture.StreamType.STREAM_TYPE_DEPTH,
                    streamConfiguration.Resolution.Width, streamConfiguration.Resolution.Height,
                    streamConfiguration.FrameRate);
            }
        }

        public void Configure()
        {
            device = nativeSense.SenseManager.QueryCaptureManager().QueryDevice();
        }

        public bool ShouldBeStarted
        {
            get { return configuration.Image.ColorEnabled || configuration.Image.DepthEnabled; }
        }

        public void Stop()
        {
            // Nothing to doa
        }

        public void Process(DeterminerData.Builder determinerData)
        {
            PXCMCapture.Sample realSenseSample = nativeSense.SenseManager.QuerySample();

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
            var uvMap = new PXCMPointF32[realSenseSample.depth.info.width*realSenseSample.depth.info.height];
            projection.QueryUVMap(realSenseSample.depth, uvMap);
            projection.Dispose();

            return uvMap;
        }

        public class Builder
        {
            private RealSenseFactory factory;
            private NativeSense nativeSense;
            private RealSenseConfiguration configuration;

            public Builder WithFactory(RealSenseFactory factory)
            {
                this.factory = factory;
                return this;
            }

            public Builder WithNativeSense(NativeSense nativeSense)
            {
                this.nativeSense = nativeSense;
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
                nativeSense.Check(Preconditions.IsNotNull,
                    "The RealSense manager must be set in order to create the hands determiner component");
                configuration.Check(Preconditions.IsNotNull,
                    "The RealSense configuration must be set in order to create the hands determiner component");

                return new ImageDeterminerComponent(factory, nativeSense, configuration);
            }
        }
    }
}