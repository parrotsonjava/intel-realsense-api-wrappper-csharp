namespace IntelRealSenseStart.Code.RealSense.Factory
{
    public class NativeFactory
    {
        private readonly PXCMSession session;

        public NativeFactory()
        {
            session = PXCMSession.CreateInstance();
        }

        public PXCMSession CurrentSession
        {
            get { return session; }
        }

        public PXCMSenseManager CreateSenseManager()
        {
            return session.CreateSenseManager();
        }
    }
}