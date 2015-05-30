using IntelRealSenseStart.Code.RealSense.Data.Determiner;

namespace IntelRealSenseStart.Code.RealSense.Component.Determiner.Face
{
    public interface FaceComponent
    {
        void EnableFeatures();

        void Configure(PXCMFaceConfiguration moduleConfiguration);

        void Process(int index, PXCMFaceData.Face face, FaceDeterminerData.Builder faceDeterminerData);

        void Stop(PXCMFaceData faceData);
    }
}