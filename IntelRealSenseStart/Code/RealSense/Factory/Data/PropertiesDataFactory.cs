using IntelRealSenseStart.Code.RealSense.Data.Properties;

namespace IntelRealSenseStart.Code.RealSense.Factory.Data
{
    public class PropertiesDataFactory
    {
        public RealSenseProperties.Builder RealSense()
        {
            return new RealSenseProperties.Builder();
        }

        public VideoProperties.Builder Video()
        {
            return new VideoProperties.Builder();
        }

        public VideoDeviceProperties.Builder VideoDevice()
        {
            return new VideoDeviceProperties.Builder();
        }

        public AudioProperties.Builder Audio()
        {
            return new AudioProperties.Builder();
        }

        public AudioInputDeviceProperties.Builder AudioDevice()
        {
            return new AudioInputDeviceProperties.Builder();
        }

        public SpeechRecognitionModuleProperties.Builder SpeechRecognitionModule()
        {
            return new SpeechRecognitionModuleProperties.Builder();
        }
        public SpeechSynthesisModuleProperties.Builder SpeechSynthesisModule()
        {
            return new SpeechSynthesisModuleProperties.Builder();
        }

        public SpeechRecognitionProfileProperties.Builder SpeechRecognitionProfile()
        {
            return new SpeechRecognitionProfileProperties.Builder();
        }

        public SpeechSynthesisProfileProperties.Builder SpeechSynthesisProfile()
        {
            return new SpeechSynthesisProfileProperties.Builder();
        }

        public StreamProperties.Builder Stream()
        {
            return new StreamProperties.Builder();
        }
    }
}