using System;
using System.Collections.Generic;
using System.Linq;
using IntelRealSenseStart.Code.RealSense.Exception;

namespace IntelRealSenseStart.Code.RealSense.Data.Properties
{
    public class RealSenseProperties
    {
        public static readonly RealSenseProperties DEFAULT_PROPERTIES;

        private readonly List<VideoDeviceProperties> videoDevices;
        private readonly List<AudioDeviceProperties> audioDevices; 

        static RealSenseProperties()
        {
            DEFAULT_PROPERTIES = new RealSenseProperties();
        }

        private RealSenseProperties()
        {
            videoDevices = new List<VideoDeviceProperties>();
            audioDevices = new List<AudioDeviceProperties>();
        }

        public List<VideoDeviceProperties> VideoDevices
        {
            get { return videoDevices; }
        }

        public List<AudioDeviceProperties> AudioDevices
        {
            get { return audioDevices; }
        }

        public VideoDeviceProperties FindDeviceBy(Func<VideoDeviceProperties, bool> selectorFunction)
        {
            var properties = VideoDevices.First(selectorFunction);
            if (properties == null)
            {
                throw new RealSenseException(String.Format("No camera with the specified selector is attached"));
            }
            return properties;
        }

        public class Builder
        {
            private readonly RealSenseProperties realSenseProperties;
            
            public Builder()
            {
                realSenseProperties = new RealSenseProperties();   
            }

            public Builder WithVideoDeviceProperties(VideoDeviceProperties.Builder videoDeviceProperties)
            {
                realSenseProperties.VideoDevices.Add(videoDeviceProperties.Build());
                return this;
            }

            public Builder WithAudioDeviceProperties(AudioDeviceProperties.Builder audioDeviceProperties)
            {
                realSenseProperties.AudioDevices.Add(audioDeviceProperties.Build());
                return this;
            }

            public RealSenseProperties Build()
            {
                return realSenseProperties;
            }
        }


    }
}