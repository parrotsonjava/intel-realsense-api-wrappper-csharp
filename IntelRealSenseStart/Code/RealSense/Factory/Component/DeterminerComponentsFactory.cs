using IntelRealSenseStart.Code.RealSense.Component.Determiner;
using IntelRealSenseStart.Code.RealSense.Component.Determiner.Builder;

namespace IntelRealSenseStart.Code.RealSense.Factory.Component
{
    public class DeterminerComponentsFactory
    {
        public ImageDeterminerComponent.Builder Image()
        {
            return new ImageDeterminerComponent.Builder();
        }

        public HandsDeterminerComponent.Builder Hands()
        {
            return new HandsDeterminerComponent.Builder();
        }

        public FaceDeterminerComponent.Builder Face()
        {
            return new FaceDeterminerComponent.Builder();
        }

        public DeviceDeterminerComponent.Builder Device()
        {
            return new DeviceDeterminerComponent.Builder();
        }

        public SpeechRecognitionDeterminerComponent.Builder SpeechRecognition()
        {
            return new SpeechRecognitionDeterminerComponent.Builder(new GrammarBuilder());
        }
    }
}