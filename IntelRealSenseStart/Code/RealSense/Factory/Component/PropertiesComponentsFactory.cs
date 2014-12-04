using IntelRealSenseStart.Code.RealSense.Component.Property;

namespace IntelRealSenseStart.Code.RealSense.Factory.Component
{
    public class PropertiesComponentsFactory
    {
        public DevicePropertiesDeterminer.Builder Device()
        {
            return new DevicePropertiesDeterminer.Builder();
        }
    }
}