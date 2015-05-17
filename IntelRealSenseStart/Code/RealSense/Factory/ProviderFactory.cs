using IntelRealSenseStart.Code.RealSense.Provider;

namespace IntelRealSenseStart.Code.RealSense.Factory
{
    public class ProviderFactory
    {
        public NativeSense.Builder NativeSense()
        {
            return new NativeSense.Builder();
        }
    }
}