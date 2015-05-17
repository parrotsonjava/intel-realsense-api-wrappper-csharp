using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using IntelRealSenseStart.Code.RealSense.Data.Properties;
using IntelRealSenseStart.Code.RealSense.Factory;
using IntelRealSenseStart.Code.RealSense.Helper;
using IntelRealSenseStart.Code.RealSense.Provider;

namespace IntelRealSenseStart.Code.RealSense.Component.Property
{
    public class AudioModulePropertiesDeterminer : PropertiesComponent<AudioProperties.Builder>
    {
        private readonly RealSenseFactory factory;
        private readonly PXCMSession session;

        private AudioModulePropertiesDeterminer(RealSenseFactory factory, NativeSense nativeSense)
        {
            this.factory = factory;
            session = nativeSense.Session;
        }

        public void UpdateProperties(AudioProperties.Builder audioProperties)
        {
            DetermineAudioModules(audioProperties);
        }

        private void DetermineAudioModules(AudioProperties.Builder audioProperties)
        {
            DetermineModules(audioProperties);
        }

        private void DetermineModules(AudioProperties.Builder audioProperties)
        {
            GetSpeechRecognitionModules().Select(GetAudioModule).Do(audioModule => audioProperties.WithModule(audioModule));
        }

        private IEnumerable<PXCMSession.ImplDesc> GetSpeechRecognitionModules()
        {
            var modules = new List<PXCMSession.ImplDesc>();
            var moduleDescription = new PXCMSession.ImplDesc();
            moduleDescription.cuids[0] = PXCMSpeechRecognition.CUID;
            for (int i = 0;; i++)
            {
                PXCMSession.ImplDesc module;
                if (session.QueryImpl(moduleDescription, i, out module) < pxcmStatus.PXCM_STATUS_NO_ERROR)
                {
                    break;
                }

                modules.Add(module);
            }
            return modules;
        }

        private AudioModuleProperties.Builder GetAudioModule(PXCMSession.ImplDesc module)
        {
            return factory.Data.Properties.AudioModule()
                .WithModuleName(module.friendlyName)
                .WithDeviceInfo(module)
                .WithSupportedLanguages(GetSupportedLanguages(module));
        }

        private List<PXCMSpeechRecognition.LanguageType> GetSupportedLanguages(PXCMSession.ImplDesc module)
        {
            var supportedLanguages = new List<PXCMSpeechRecognition.LanguageType>();

            var languageDescription = new PXCMSession.ImplDesc();
            languageDescription.cuids[0] = PXCMSpeechRecognition.CUID;
            languageDescription.iuid = module.iuid;

            PXCMSpeechRecognition speechRecognition;
            if (session.CreateImpl<PXCMSpeechRecognition>(languageDescription, out speechRecognition) <
                pxcmStatus.PXCM_STATUS_NO_ERROR)
            {
                return new List<PXCMSpeechRecognition.LanguageType>();
            }

            for (var i = 0; ; i++)
            {
                PXCMSpeechRecognition.ProfileInfo profileInfo;
                if (speechRecognition.QueryProfile(i, out profileInfo) < pxcmStatus.PXCM_STATUS_NO_ERROR)
                {
                    break;
                }

                supportedLanguages.Add(profileInfo.language);
            }
            speechRecognition.Dispose();
            return supportedLanguages;
        }

        public class Builder
        {
            private RealSenseFactory factory;
            private NativeSense nativeSense;

            public Builder WithFactory(RealSenseFactory factory)
            {
                this.factory = factory;
                return this;
            }

            public Builder WithNativeSense(NativeSense nativeSense)
            {
                this.nativeSense = nativeSense;
                return this;
            }

            public AudioModulePropertiesDeterminer Build()
            {
                factory.Check(Preconditions.IsNotNull,
                    "The factory must be set in order to create the audio language properties determiner");
                nativeSense.Check(Preconditions.IsNotNull,
                    "The native set must be set in order to create the audio language properties determiner");

                return new AudioModulePropertiesDeterminer(factory, nativeSense);
            }
        }
    }
}