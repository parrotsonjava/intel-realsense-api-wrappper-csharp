using IntelRealSenseStart.Code.RealSense.Component.Output;

namespace IntelRealSenseStart.Code.RealSense.Factory.Component
{
    public class OutputComponentsFactory
    {
        public SpeechSynthesisOutputComponent.Builder SpeechSynthesis()
        {
            return new SpeechSynthesisOutputComponent.Builder();
        }
    }
}
