namespace IntelRealSenseStart.Code.RealSense.Factory
{
    public class NativeFactory
    {
        public PXCMSession Session()
        {
            return PXCMSession.CreateInstance();
        }

        public PXCMAudioSource AudioSource(PXCMSession session)
        {
            return session.CreateAudioSource();
        }

        public PXCMSenseManager SenseManager(PXCMSession session)
        {
            return session.CreateSenseManager();
        }
    }
}