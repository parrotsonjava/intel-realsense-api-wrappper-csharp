using IntelRealSenseStart.Code.RealSense.Config.RealSense;
using IntelRealSenseStart.Code.RealSense.Data.Determiner;
using IntelRealSenseStart.Code.RealSense.Helper;

namespace IntelRealSenseStart.Code.RealSense.Component.Determiner.Face
{
    public class PulseDeterminerComponent : FaceComponent
    {
        private readonly RealSenseConfiguration configuration;

        private PulseDeterminerComponent(RealSenseConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void EnableFeatures()
        {
            // Nothing to do
        }

        public void Configure(PXCMFaceConfiguration moduleConfiguration)
        {
            ConfigurePulse(moduleConfiguration);
        }

        private void ConfigurePulse(PXCMFaceConfiguration moduleConfiguration)
        {
            if (!configuration.FaceDetection.UsePulse)
            {
                return;
            }

            var pulseConfig = moduleConfiguration.QueryPulse();
            pulseConfig.properties.maxTrackedFaces = 4;
            pulseConfig.Enable();
        }

        public void Process(int index, PXCMFaceData.Face face, FaceDeterminerData.Builder faceDeterminerData)
        {
            faceDeterminerData.WithPulse(GetPulseData(face));
        }

        private PXCMFaceData.PulseData GetPulseData(PXCMFaceData.Face face)
        {
            if (configuration.FaceDetection.UsePulse)
            {
                return face.QueryPulse();
            }
            return null;
        }

        public void Stop(PXCMFaceData faceData)
        {
            // Nothing to do
        }

        public class Builder
        {
            private RealSenseConfiguration configuration;

            public Builder WithConfiguration(RealSenseConfiguration configuration)
            {
                this.configuration = configuration;
                return this;
            }

            public PulseDeterminerComponent Build()
            {
                configuration.Check(Preconditions.IsNotNull,
                    "The RealSense configuration must be set in order to create the face landmarks determiner component");

                return new PulseDeterminerComponent(configuration);
            }
        }
    }
}