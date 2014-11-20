using IntelRealSenseStart.Code.RealSense.Event;

namespace IntelRealSenseStart.Code.RealSense.Component
{
    internal interface Component
    {
        bool ShouldBeStarted { get; }

        void EnableFeatures();

        void Configure();

        void Process(FrameEventArgs.Builder frameEvent);
    }
}