﻿using System;

namespace IntelRealSenseStart.Code.RealSense.Config.RealSense
{
    public class SpeechRecognitionConfiguration
    {
        private const float DEFAULT_VOLUME = 0.2f;

        private float volume;
        private String grammmar;


        public float Volume
        {
            get { return volume; }
        }

        public bool UsingGrammar
        {
            get { return grammmar != null; }
        }

        public String Grammar
        {
            get { return grammmar; }
        }

        public class Builder
        {
            private readonly SpeechRecognitionConfiguration configuration;

            public Builder()
            {
                configuration = new SpeechRecognitionConfiguration();
                WithDefaultValues();
            }

            public Builder WithDefaultValues()
            {
                return UsingDictation().WithVolume(DEFAULT_VOLUME);
            }

            private Builder WithVolume(float volume)
            {
                configuration.volume = volume;
                return this;
            }

            public Builder UsingDictation()
            {
                configuration.grammmar = null;
                return this;
            }

            public Builder UsingGrammmar(String grammar)
            {
                configuration.grammmar = grammar;
                return this;
            }

            public SpeechRecognitionConfiguration Build()
            {
                return configuration;
            }
        }
    }
}