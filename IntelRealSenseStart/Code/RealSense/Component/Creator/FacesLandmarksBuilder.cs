using System.Collections.Generic;
using IntelRealSenseStart.Code.RealSense.Data.Common;
using IntelRealSenseStart.Code.RealSense.Data.Determiner;
using IntelRealSenseStart.Code.RealSense.Data.Event;
using IntelRealSenseStart.Code.RealSense.Factory;
using IntelRealSenseStart.Code.RealSense.Helper;

namespace IntelRealSenseStart.Code.RealSense.Component.Creator
{
    public class FacesLandmarksBuilder
    {
        private readonly RealSenseFactory factory;

        public FacesLandmarksBuilder(RealSenseFactory factory)
        {
            this.factory = factory;
        }

        public FacesLandmarksData GetLandmarkData(List<FaceData> facesData)
        {
            var facesLandmarks = factory.Data.Events.FacesLandmarks();
            facesData.Do(faceData => facesLandmarks.WithFaceLandmarks(GetFaceLandmarks(faceData)));
            return facesLandmarks.Build();
        }

        private FaceLandmarksData.Builder GetFaceLandmarks(FaceData faceData)
        {
            var faceLandmarks = factory.Data.Events.FaceLandmarks();
            0.To(faceData.LandmarkPoints.Length - 1).ToArray().Do(index =>
                faceLandmarks.WithDetectionPoint(
                    GetLandmarkName(index),
                    GetDetectionPoint(faceData.LandmarkPoints[index])));
            return faceLandmarks;
        }

        private FaceLandmark GetLandmarkName(int index)
        {
            return (FaceLandmark) index;
        }

        private DetectionPoint.Builder GetDetectionPoint(PXCMFaceData.LandmarkPoint landmarkPoint)
        {
            return factory.Data.Events.DetectionPoint()
                .WithImagePosition(GetPoint2DFrom(landmarkPoint.image))
                .WithWorldPosition(GetPoint3DFrom(landmarkPoint.world));
        }

        private Point2D.Builder GetPoint2DFrom(PXCMPointF32 point)
        {
            return factory.Data.Common.Point2D().From(point);
        }

        private Point3D.Builder GetPoint3DFrom(PXCMPoint3DF32 point)
        {
            return factory.Data.Common.Point3D().From(point);
        }

        public class Builder
        {
            private readonly FacesLandmarksBuilder facesLandmarksBuilder;

            public Builder(RealSenseFactory factory)
            {
                facesLandmarksBuilder = new FacesLandmarksBuilder(factory);
            }

            public FacesLandmarksBuilder Build()
            {
                return facesLandmarksBuilder;
            }
        }
    }
}