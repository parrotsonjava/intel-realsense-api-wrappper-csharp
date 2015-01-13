using IntelRealSenseStart.Code.RealSense.Provider;

namespace IntelRealSenseStart.Code.RealSense.Factory
{
    public class ProviderFactory
    {
        public SenseManagerProvider.Builder SenseManager()
        {
            return new SenseManagerProvider.Builder();
        }
    }
}