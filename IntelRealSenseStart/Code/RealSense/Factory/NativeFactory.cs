namespace IntelRealSenseStart.Code.RealSense.Factory
{
    public class NativeFactory
    {
        public PXCMSession Session()
        {
            return PXCMSession.CreateInstance();
        }

        public PXCMSenseManager SenseManager(PXCMSession session)
        {
            return session.CreateSenseManager();
        }
    }
}