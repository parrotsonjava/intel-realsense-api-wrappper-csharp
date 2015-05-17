using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntelRealSenseStart.Code.RealSense.Config.RealSense
{
    public class SpeechConfiguration
    {

        private bool usingDictation;

        public bool UsingDictation
        {
            get { return usingDictation; }
        }

        public class Builder
        {
            private SpeechConfiguration configuration;

            public Builder()
            {
                configuration = new SpeechConfiguration();
            }

            public Builder UsingDictation()
            {
                configuration.usingDictation = true;
                return this;
            }

            public SpeechConfiguration Build()
            {
                return configuration;
            }
        }
    }
}