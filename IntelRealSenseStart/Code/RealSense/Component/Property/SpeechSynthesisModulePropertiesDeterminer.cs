using System.Collections.Generic;
using System.Linq;
using IntelRealSenseStart.Code.RealSense.Data.Properties;
using IntelRealSenseStart.Code.RealSense.Factory;
using IntelRealSenseStart.Code.RealSense.Helper;
using IntelRealSenseStart.Code.RealSense.Native;
using IntelRealSenseStart.Code.RealSense.Provider;

namespace IntelRealSenseStart.Code.RealSense.Component.Property
{
    public class SpeechSynthesisModulePropertiesDeterminer : PropertiesComponent<AudioProperties.Builder>
    {
        private readonly RealSenseFactory factory;
        private readonly PXCMSession session;

        private SpeechSynthesisModulePropertiesDeterminer(RealSenseFactory factory, NativeSense nativeSense)
        {
            this.factory = factory;
            session = nativeSense.Session;
        }

        public void UpdateProperties(AudioProperties.Builder properties)
        {
            session.GetModules(PXCMSpeechSynthesis.CUID)
                .Select(GetAudioModule)
                .Do(module => properties.WithSpeechSynthesisModule(module));
        }

        private SpeechSynthesisModuleProperties.Builder GetAudioModule(PXCMSession.ImplDesc module)
        {
            return factory.Data.Properties.SpeechSynthesisModule()
                .WithModuleName(module.friendlyName)
                .WithDeviceInfo(module)
                .WithProfiles(GetProfiles(module));
        }

        private List<SpeechSynthesisProfileProperties.Builder> GetProfiles(PXCMSession.ImplDesc module)
        {
            var profiles = new List<SpeechSynthesisProfileProperties.Builder>();

            PXCMSpeechSynthesis speechSynthesis;
            if (session.CreateImpl(module, out speechSynthesis) < pxcmStatus.PXCM_STATUS_NO_ERROR)
            {
                return profiles;
            }

            for (var i = 0; ; i++)
            {
                PXCMSpeechSynthesis.ProfileInfo profile;
                if (speechSynthesis.QueryProfile(i, out profile) < pxcmStatus.PXCM_STATUS_NO_ERROR)
                {
                    break;
                }

                profiles.Add(factory.Data.Properties.SpeechSynthesisProfile()
                    .WithProfile(profile)
                    .WithLanguage(profile.language));
            }

            speechSynthesis.Dispose();
            return profiles;
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

            public SpeechSynthesisModulePropertiesDeterminer Build()
            {
                factory.Check(Preconditions.IsNotNull,
                    "The factory must be set in order to create the audio language properties determiner");
                nativeSense.Check(Preconditions.IsNotNull,
                    "The native set must be set in order to create the audio language properties determiner");

                return new SpeechSynthesisModulePropertiesDeterminer(factory, nativeSense);
            }
        }
    }
}