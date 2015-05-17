using IntelRealSenseStart.Code.RealSense.Component.Property;

namespace IntelRealSenseStart.Code.RealSense.Factory.Component
{
    public class PropertiesComponentsFactory
    {
        public VideoPropertiesDeterminer.Builder VideoDeterminer()
        {
            return new VideoPropertiesDeterminer.Builder();
        }

        public VideoDevicePropertiesDeterminer.Builder VideoDeviceDeterminer()
        {
            return new VideoDevicePropertiesDeterminer.Builder();
        }

        public AudioPropertiesDeterminer.Builder AudioDeterminer()
        {
            return new AudioPropertiesDeterminer.Builder();
        }

        public AudioDevicePropertiesDeterminer.Builder AudioDeviceDeterminer()
        {
            return new AudioDevicePropertiesDeterminer.Builder();
        }

        public SpeechRecognitionModulePropertiesDeterminer.Builder SpeechRecognitionModuleDeterminer()
        {
            return new SpeechRecognitionModulePropertiesDeterminer.Builder();
        }

        public SpeechSynthesisModulePropertiesDeterminer.Builder SpeechSynthesisModuleDeterminer()
        {
            return new SpeechSynthesisModulePropertiesDeterminer.Builder();
        }
    }
}