using IntelRealSenseStart.Code.RealSense.Config.RealSense;
using IntelRealSenseStart.Code.RealSense.Data.Determiner;
using IntelRealSenseStart.Code.RealSense.Exception;
using IntelRealSenseStart.Code.RealSense.Helper;

namespace IntelRealSenseStart.Code.RealSense.Component.Determiner
{
    public class DeviceDeterminerComponent : DeterminerComponent
    {
        private readonly PXCMSenseManager manager;
        private readonly RealSenseConfiguration configuration;

        private PXCMCapture.Device device;

        private DeviceDeterminerComponent(PXCMSenseManager manager, RealSenseConfiguration configuration)
        {
            this.manager = manager;
            this.configuration = configuration;
        }

        public void EnableFeatures()
        {
            var deviceProperties = configuration.Device.VideoDevice.Device;
            if (deviceProperties != null)
            {
                manager.captureManager.FilterByDeviceInfo(deviceProperties.DeviceInfo);
            }
        }

        public void Configure()
        {
            PXCMCapture.DeviceInfo deviceInfo;
            var queryCaptureManager = manager.QueryCaptureManager();
            device = queryCaptureManager.QueryDevice();

            if (device == null)
            {
                throw new RealSenseException("No device found for the selected device");
            }

            device.QueryDeviceInfo(out deviceInfo);

            if (deviceInfo == null)
            {
                throw new RealSenseException("No device info found for the selected device");
            }

            manager.captureManager.device.SetDepthConfidenceThreshold(1);
            manager.captureManager.device.SetMirrorMode(PXCMCapture.Device.MirrorMode.MIRROR_MODE_HORIZONTAL);
            manager.captureManager.device.SetIVCAMFilterOption(6);
        }

        public void Process(DeterminerData.Builder determinerData)
        {
            determinerData.WithDevice(device);
        }

        public bool ShouldBeStarted
        {
            get { return true; }
        }

        public class Builder
        {
            private PXCMSenseManager manager;
            private RealSenseConfiguration configuration;

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

            public DeviceDeterminerComponent Build()
            {
                manager.Check(Preconditions.IsNotNull, "The RealSense manager must be set to create the device component");
                configuration.Check(Preconditions.IsNotNull, "The RealSense configuration must be set to create the device component");

                return new DeviceDeterminerComponent(manager, configuration);
            }
        }
    }
}