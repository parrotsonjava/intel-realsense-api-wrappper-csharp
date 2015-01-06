using IntelRealSenseStart.Code.RealSense.Data.Determiner;

namespace IntelRealSenseStart.Code.RealSense.Component.Determiner
{
    internal interface DeterminerComponent
    {
        bool ShouldBeStarted { get; }

        void EnableFeatures();

        void Configure();

        void Process(DeterminerData.Builder determinerData);
    }
}