using IntelRealSenseStart.Code.RealSense.Factory;

namespace IntelRealSenseStart.Code.RealSense.Provider
{
    public class NativeSense
    {
        private NativeFactory nativeFactory;

        private PXCMSession session;
        private PXCMSenseManager senseManager;

        private NativeSense()
        { }

        public void Initialize()
        {
            session = nativeFactory.Session();
            senseManager = nativeFactory.SenseManager(session);
        }

        public PXCMSession Session
        {
            get { return session; }
        }

        public PXCMSenseManager SenseManager
        {
            get { return senseManager; }
        }

        public class Builder
        {
            private readonly NativeSense nativeSense;

            public Builder()
            {
                nativeSense = new NativeSense();
            }

            public Builder WithFactory(NativeFactory nativeFactory)
            {
                nativeSense.nativeFactory = nativeFactory;
                return this;
            }

            public NativeSense Build()
            {
                return nativeSense;
            }
        }
    }
}