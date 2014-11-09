namespace IntelRealSenseStart.Code
{
    internal class RealSenseFactory
    {
        private static volatile RealSenseFactory instance;
        private readonly PXCMSenseManager realSenseManager;

        private RealSenseFactory()
        {
            realSenseManager = PXCMSession.CreateInstance().CreateSenseManager();
        }

        private static RealSenseFactory Instance
        {
            get { return instance ?? (instance = new RealSenseFactory()); }
        }

        public static RealSenseHandsDeterminer GetHandsDeterminer()
        {
            return Instance.GetRealSenseHandsDeterminer();
        }

        private RealSenseHandsDeterminer GetRealSenseHandsDeterminer()
        {
            return new RealSenseHandsDeterminer(realSenseManager, this);
        }

        public HandBitmapBuilder CreateHandBitmapBuilder()
        {
            return new HandBitmapBuilder();
        }
    }
}