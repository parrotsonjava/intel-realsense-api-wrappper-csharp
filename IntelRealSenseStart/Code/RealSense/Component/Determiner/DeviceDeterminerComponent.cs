using IntelRealSenseStart.Code.RealSense.Event;
using IntelRealSenseStart.Code.RealSense.Exception;
using IntelRealSenseStart.Code.RealSense.Helper;

namespace IntelRealSenseStart.Code.RealSense.Component.Determiner
{
    public class DeviceDeterminerComponent : DeterminerComponent
    {
        private readonly PXCMSenseManager manager;
        private PXCMCapture.Device device;

        private DeviceDeterminerComponent(PXCMSenseManager manager)
        {
            this.manager = manager;
        }

        public void EnableFeatures()
        {
            // Nothing to do
        }

        public void Configure()
        {
            PXCMCapture.DeviceInfo deviceInfo;
            device = manager.QueryCaptureManager().QueryDevice();
            device.QueryDeviceInfo(out deviceInfo);

            if (deviceInfo == null || deviceInfo.model != PXCMCapture.DeviceModel.DEVICE_MODEL_IVCAM)
            {
                throw new RealSenseException("No device info found");
            }

            manager.captureManager.device.SetDepthConfidenceThreshold(1);
            manager.captureManager.device.SetMirrorMode(PXCMCapture.Device.MirrorMode.MIRROR_MODE_HORIZONTAL);
            manager.captureManager.device.SetIVCAMFilterOption(6);
        }

        public void Process(FrameEventArgs.Builder frameEvent)
        {
            frameEvent.WithDevice(device);
        }

        public bool ShouldBeStarted
        {
            get { return true; }
        }

        public class Builder
        {
            private PXCMSenseManager manager;

            public Builder WithManager(PXCMSenseManager manager)
            {
                this.manager = manager;
                return this;
            }

            public DeviceDeterminerComponent Build()
            {
                manager.CheckState(Preconditions.IsNotNull, "The RealSense manager must be set to create the device component");
                return new DeviceDeterminerComponent(manager);
            }
        }
    }
}