using System.Collections.Generic;
using IntelRealSenseStart.Code.RealSense.Data.Common;
using IntelRealSenseStart.Code.RealSense.Data.Determiner;
using IntelRealSenseStart.Code.RealSense.Data.Event;
using IntelRealSenseStart.Code.RealSense.Factory;
using IntelRealSenseStart.Code.RealSense.Helper;
using FaceData = IntelRealSenseStart.Code.RealSense.Data.Event.FaceData;
using FacesData = IntelRealSenseStart.Code.RealSense.Data.Event.FacesData;

namespace IntelRealSenseStart.Code.RealSense.Component.Creator
{
    public class FacesBuilder
    {
        private readonly RealSenseFactory factory;

            public FacesBuilder(RealSenseFactory factory)
        {
            this.factory = factory;
        }

        public FacesData GetFacesData(List<FaceDeterminerData> facesDeterminerData)
        {
            var facesLandmarks = factory.Data.Events.Faces();
            facesDeterminerData.Do(faceData => facesLandmarks.WithFaceLandmarks(GetFaceData(faceData)));
            return facesLandmarks.Build();
        }

        private FaceData.Builder GetFaceData(FaceDeterminerData faceDeterminerData)
        {
            var face = factory.Data.Events.Face();
            if (faceDeterminerData.LandmarkPoints != null)
            {
                0.To(faceDeterminerData.LandmarkPoints.Length - 1).ToArray().Do(index =>
                    face.WithDetectionPoint(
                        GetLandmarkName(index),
                        GetDetectionPoint(faceDeterminerData.LandmarkPoints[index])));
            }

            return face.WithPulseData(faceDeterminerData.PulseData)
                .WithFaceId(faceDeterminerData.FaceId)
                .WithRecognizedId(GetRecognizedId(faceDeterminerData));
        }

        private FaceLandmark GetLandmarkName(int index)
        {
            return (FaceLandmark) index;
        }

        private DetectionPoint.Builder GetDetectionPoint(PXCMFaceData.LandmarkPoint landmarkPoint)
        {
            return factory.Data.Events.DetectionPoint()
                .WithImagePosition(GetPoint2DFrom(landmarkPoint.image, landmarkPoint.confidenceImage))
                .WithWorldPosition(GetPoint3DFrom(landmarkPoint.world, landmarkPoint.confidenceWorld));
        }

        private Point2D.Builder GetPoint2DFrom(PXCMPointF32 point, int confidence)
        {
            return factory.Data.Common.Point2D().From(point).WithConfidence(confidence);
        }

        private Point3D.Builder GetPoint3DFrom(PXCMPoint3DF32 point, int confidence)
        {
            return factory.Data.Common.Point3D().From(point).WithConfidence(confidence);
        }

        private int? GetRecognizedId(FaceDeterminerData faceDeterminerData)
        {
            if (faceDeterminerData.RecognizedId == -1)
            {
                return null;
            }
            return faceDeterminerData.RecognizedId;
        }

        public class Builder
        {
            private readonly FacesBuilder facesLandmarksBuilder;

            public Builder(RealSenseFactory factory)
            {
                facesLandmarksBuilder = new FacesBuilder(factory);
            }

            public FacesBuilder Build()
            {
                return facesLandmarksBuilder;
            }
        }
    }
}