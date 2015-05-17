using IntelRealSenseStart.Code.RealSense.Component.Property;

namespace IntelRealSenseStart.Code.RealSense.Factory.Component
{
    public class PropertiesComponentsFactory
    {
        public VideoDevicePropertiesDeterminer.Builder VideoDevice()
        {
            return new VideoDevicePropertiesDeterminer.Builder();
        }

        public AudioDevicePropertiesDeterminer.Builder AudioDevice()
        {
            return new AudioDevicePropertiesDeterminer.Builder();
        }
    }
}