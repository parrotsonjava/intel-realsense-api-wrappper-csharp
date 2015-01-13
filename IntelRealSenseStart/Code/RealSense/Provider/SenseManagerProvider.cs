namespace IntelRealSenseStart.Code.RealSense.Provider
{
    public class SenseManagerProvider
    {
        private SenseManagerProvider()
        {
        }

        public PXCMSenseManager SenseManager { get; private set; }

        public class Builder
        {
            private readonly SenseManagerProvider senseManagerProvider;

            public Builder()
            {
                senseManagerProvider = new SenseManagerProvider();
            }

            public Builder WithSenseManager(PXCMSenseManager senseManager)
            {
                senseManagerProvider.SenseManager = senseManager;
                return this;
            }

            public SenseManagerProvider Build()
            {
                return senseManagerProvider;
            }
        }
    }
}
