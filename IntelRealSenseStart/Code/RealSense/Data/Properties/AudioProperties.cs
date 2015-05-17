using System.Collections.Generic;

namespace IntelRealSenseStart.Code.RealSense.Data.Properties
{
    public class AudioProperties
    {
        private readonly List<AudioInputDeviceProperties> audioInputInputDeviceProperties;

        private readonly List<SpeechRecognitionModuleProperties> speechRecognitionModuleProperties;
        private readonly List<SpeechSynthesisModuleProperties> speechSynthesisModuleProperties;

        private AudioProperties()
        {
            audioInputInputDeviceProperties = new List<AudioInputDeviceProperties>();
            speechRecognitionModuleProperties = new List<SpeechRecognitionModuleProperties>();
            speechSynthesisModuleProperties = new List<SpeechSynthesisModuleProperties>();
        }

        public List<AudioInputDeviceProperties> InputDevices
        {
            get { return audioInputInputDeviceProperties; }
        }

        public List<SpeechRecognitionModuleProperties> SpeechRecognitionModules
        {
            get { return speechRecognitionModuleProperties; }
        }

        public List<SpeechSynthesisModuleProperties> SpeechSynthesisModules
        {
            get { return speechSynthesisModuleProperties; }
        }

        public class Builder
        {
            private readonly AudioProperties audioProperties;

            public Builder()
            {
                audioProperties = new AudioProperties();
            }

            public Builder WithAudioInputDevice(AudioInputDeviceProperties.Builder device)
            {
                audioProperties.audioInputInputDeviceProperties.Add(device.Build());
                return this;
            }

            public Builder WithSpeechRecognitionModule(SpeechRecognitionModuleProperties.Builder module)
            {
                audioProperties.speechRecognitionModuleProperties.Add(module.Build());
                return this;
            }

            public Builder WithSpeechSynthesisModule(SpeechSynthesisModuleProperties.Builder module)
            {
                audioProperties.speechSynthesisModuleProperties.Add(module.Build());
                return this;
            }

            public AudioProperties Build()
            {
                return audioProperties;
            }
        }
    }
}