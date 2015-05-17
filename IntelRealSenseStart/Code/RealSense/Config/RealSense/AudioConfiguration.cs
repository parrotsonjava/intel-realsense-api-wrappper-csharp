using System;
using IntelRealSenseStart.Code.RealSense.Data.Properties;

namespace IntelRealSenseStart.Code.RealSense.Config.RealSense
{
    public class AudioConfiguration
    {
        public static readonly AudioConfiguration DEFAULT_CONFIGURATION;

        private Func<AudioInputDeviceProperties, bool> inputDeviceSelectorFunction;
        private Func<SpeechRecognitionProfileProperties, bool> speechRecognitionProfileSelectorFunction;
        private Func<SpeechSynthesisProfileProperties, bool> speechSynthesisProfileSelectorFunction;

        static AudioConfiguration()
        {
            DEFAULT_CONFIGURATION = new Builder().WithDefaultConfiguration().Build();
        }

        private AudioConfiguration()
        {
        }

        public Func<AudioInputDeviceProperties, bool> InputDeviceSelectorFunction
        {
            get { return inputDeviceSelectorFunction; }
        }

        public Func<SpeechRecognitionProfileProperties, bool> SpeechRecognitionProfileSelectorFunction
        {
            get { return speechRecognitionProfileSelectorFunction; }
        }


        public Func<SpeechSynthesisProfileProperties, bool> SpeechSynthesisProfileSelectorFunction
        {
            get { return speechSynthesisProfileSelectorFunction; }
        }

        public class Builder
        {
            private readonly AudioConfiguration configuration;

            public Builder()
            {
                configuration = new AudioConfiguration();
                WithDefaultConfiguration();
            }

            public Builder WithDefaultConfiguration()
            {
                configuration.inputDeviceSelectorFunction = device => true;
                configuration.speechRecognitionProfileSelectorFunction = profile => true;
                configuration.speechSynthesisProfileSelectorFunction = profile => true;
                return this;
            }

            public Builder UsingAudioInputDevice(Func<AudioInputDeviceProperties, bool> selectorFunction)
            {
                configuration.inputDeviceSelectorFunction = selectorFunction;
                return this;
            }

            public Builder UsingSpeechRecongitionProfile(Func<SpeechRecognitionProfileProperties, bool> selectorFunction)
            {
                configuration.speechRecognitionProfileSelectorFunction = selectorFunction;
                return this;
            }

            public Builder UsingSpeechSynthesisProfile(Func<SpeechSynthesisProfileProperties, bool> selectorFunction)
            {
                configuration.speechSynthesisProfileSelectorFunction = selectorFunction;
                return this;
            }

            public AudioConfiguration Build()
            {
                return configuration;
            }
        }
    }
}