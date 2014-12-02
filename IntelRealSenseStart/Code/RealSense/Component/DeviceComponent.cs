using IntelRealSenseStart.Code.RealSense.Event;

namespace IntelRealSenseStart.Code.RealSense.Component
{
    public class DeviceComponent : Component
    {
        private readonly PXCMSenseManager manager;
        private PXCMCapture.Device device;

        private DeviceComponent(PXCMSenseManager manager)
        {
            this.manager = manager;
        }

        public void EnableFeatures()
        {
            // Nothing to do
        }

        public void Configure()
        {
            device = manager.QueryCaptureManager().QueryDevice();
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
            public DeviceComponent Build(PXCMSenseManager manager)
            {
                return new DeviceComponent(manager);
            }
        }
    }
}