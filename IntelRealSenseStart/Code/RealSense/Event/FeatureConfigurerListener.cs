using IntelRealSenseStart.Code.RealSense.Factory.Configuration;

namespace IntelRealSenseStart.Code.RealSense.Event
{
    public delegate BUILDER FeatureConfigurationListener<out BUILDER>(DeterminerConfigurationFactory featureFactory);
}
