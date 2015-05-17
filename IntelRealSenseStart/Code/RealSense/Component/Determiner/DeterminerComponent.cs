using System;

namespace IntelRealSenseStart.Code.RealSense.Component.Determiner
{
    public interface DeterminerComponent
    {
        bool ShouldBeStarted { get; }

        void EnableFeatures();

        void Configure();

        void Stop();
    }
}
