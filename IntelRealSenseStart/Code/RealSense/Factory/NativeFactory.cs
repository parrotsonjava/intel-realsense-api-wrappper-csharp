using IntelRealSenseStart.Code.RealSense.Exception;

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
            var audioSource = session.CreateAudioSource();
            if (audioSource == null)
            {
                throw new RealSenseException("Error initializing the audio source");
            }
            return audioSource;
        }

        public PXCMSenseManager SenseManager(PXCMSession session)
        {
            return session.CreateSenseManager();
        }
    }
}