using IntelRealSenseStart.Code.RealSense.Config.RealSense;
using IntelRealSenseStart.Code.RealSense.Data.Determiner;
using IntelRealSenseStart.Code.RealSense.Helper;

namespace IntelRealSenseStart.Code.RealSense.Component.Determiner.Face
{
    public class FaceLandmarksDeterminerComponent : FaceComponent
    {
        private readonly RealSenseConfiguration configuration;

        private FaceLandmarksDeterminerComponent(RealSenseConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void Configure(PXCMFaceConfiguration moduleConfiguration)
        {
        }

        public void Process(PXCMFaceData.Face face, FaceDeterminerData.Builder faceDeterminerData)
        {
            faceDeterminerData.WithLandmarks(GetLandmarkData(face));
        }

        private PXCMFaceData.LandmarkPoint[] GetLandmarkData(PXCMFaceData.Face face)
        {
            if (configuration.FaceDetection.UseLandmarks)
            {
                var landMarks = face.QueryLandmarks();
                return GetLandmarkPoints(landMarks);
            }
            return null;
        }

        private static PXCMFaceData.LandmarkPoint[] GetLandmarkPoints(PXCMFaceData.LandmarksData landMarks)
        {
            if (landMarks == null)
            {
                return null;
            }

            PXCMFaceData.LandmarkPoint[] points;
            landMarks.QueryPoints(out points);
            return points;
        }

        public void Stop(PXCMFaceData faceData)
        {
            // Nothing to do
        }

        public class Builder
        {
            private RealSenseConfiguration configuration;

            public Builder WithConfiguration(RealSenseConfiguration configuration)
            {
                this.configuration = configuration;
                return this;
            }

            public FaceLandmarksDeterminerComponent Build()
            {
                configuration.Check(Preconditions.IsNotNull,
                    "The RealSense configuration must be set in order to create the face landmarks determiner component");

                return new FaceLandmarksDeterminerComponent(configuration);
            }
        }
    }
}