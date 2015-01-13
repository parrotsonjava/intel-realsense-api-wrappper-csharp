using IntelRealSenseStart.Code.RealSense.Config.RealSense;
using IntelRealSenseStart.Code.RealSense.Data.Determiner;
using IntelRealSenseStart.Code.RealSense.Exception;
using IntelRealSenseStart.Code.RealSense.Helper;
using IntelRealSenseStart.Code.RealSense.Manager;
using IntelRealSenseStart.Code.RealSense.Provider;

namespace IntelRealSenseStart.Code.RealSense.Component.Determiner
{
    public class DeviceDeterminerComponent : DeterminerComponent
    {
        private readonly SenseManagerProvider senseManagerProvider;
        private readonly RealSensePropertiesManager propertiesManager;
        private readonly RealSenseConfiguration configuration;

        private PXCMCapture.Device device;

        private DeviceDeterminerComponent(SenseManagerProvider senseManagerProvider,
            RealSensePropertiesManager propertiesManager, RealSenseConfiguration configuration)
        {
            this.senseManagerProvider = senseManagerProvider;
            this.propertiesManager = propertiesManager;
            this.configuration = configuration;
        }

        public void EnableFeatures()
        {
            var deviceName = configuration.Device.VideoDevice.DeviceName;
            if (deviceName != null)
            {
                var properties = propertiesManager.GetProperties();
                var deviceProperties = properties.FindDeviceByName(deviceName);

                senseManagerProvider.SenseManager.captureManager.FilterByDeviceInfo(deviceProperties.DeviceInfo);
            }
        }

        public void Configure()
        {
            PXCMCapture.DeviceInfo deviceInfo;
            var queryCaptureManager = senseManagerProvider.SenseManager.QueryCaptureManager();
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

            senseManagerProvider.SenseManager.captureManager.device.SetDepthConfidenceThreshold(1);
            senseManagerProvider.SenseManager.captureManager.device.SetMirrorMode(
                PXCMCapture.Device.MirrorMode.MIRROR_MODE_HORIZONTAL);
            senseManagerProvider.SenseManager.captureManager.device.SetIVCAMFilterOption(6);
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
            private SenseManagerProvider senseManagerProvider;
            private RealSensePropertiesManager propertiesManager;
            private RealSenseConfiguration configuration;

            public Builder WithManager(SenseManagerProvider senseManagerProvider)
            {
                this.senseManagerProvider = senseManagerProvider;
                return this;
            }

            public Builder WithPropertiesManager(RealSensePropertiesManager propertiesManager)
            {
                this.propertiesManager = propertiesManager;
                return this;
            }

            public Builder WithConfiguration(RealSenseConfiguration configuration)
            {
                this.configuration = configuration;
                return this;
            }

            public DeviceDeterminerComponent Build()
            {
                propertiesManager.Check(Preconditions.IsNotNull,
                    "The properties determiner must be set to create the device component");
                senseManagerProvider.Check(Preconditions.IsNotNull,
                    "The RealSense manager must be set to create the device component");
                configuration.Check(Preconditions.IsNotNull,
                    "The RealSense configuration must be set to create the device component");

                return new DeviceDeterminerComponent(senseManagerProvider, propertiesManager, configuration);
            }
        }
    }
}