using System.Collections.Generic;
using IntelRealSenseStart.Code.RealSense.Data.Common;
using IntelRealSenseStart.Code.RealSense.Data.Determiner;
using IntelRealSenseStart.Code.RealSense.Data.Event;
using IntelRealSenseStart.Code.RealSense.Factory;
using IntelRealSenseStart.Code.RealSense.Helper;
using HandData = IntelRealSenseStart.Code.RealSense.Data.Event.HandData;
using HandsData = IntelRealSenseStart.Code.RealSense.Data.Event.HandsData;

namespace IntelRealSenseStart.Code.RealSense.Component.Creator
{
    public class HandsBuilder
    {
        private readonly RealSenseFactory factory;

        public HandsBuilder(RealSenseFactory factory)
        {
            this.factory = factory;
        }

        public HandsData GetHandsData(List<Data.Determiner.HandData> handsData)
        {
            var handsJoints = factory.Data.Events.Hands();
            handsData.Do(handData => handsJoints.WithFaceLandmarks(GetHandJoints(handData)));
            return handsJoints.Build();
        }

        private HandData.Builder GetHandJoints(Data.Determiner.HandData handData)
        {
            var handJoints = factory.Data.Events.Hand();
            0.To(handData.Joints.Count - 1).ToArray().Do(index =>
                handJoints.WithDetectionPoint(
                    GetJointName(index),
                    GetDetectionPoint(handData.Joints[(PXCMHandData.JointType) index])));
            return handJoints;
        }

        private HandJoint GetJointName(int index)
        {
            return (HandJoint)index;
        }

        private DetectionPoint.Builder GetDetectionPoint(PXCMHandData.JointData jointData)
        {
            return factory.Data.Events.DetectionPoint()
                .WithImagePosition(GetPoint2DFrom(jointData.positionImage, jointData.confidence))
                .WithWorldPosition(GetPoint3DFrom(jointData.positionWorld, jointData.confidence));
        }

        private Point2D.Builder GetPoint2DFrom(PXCMPoint3DF32 point, int confidence)
        {
            return factory.Data.Common.Point2D().From(point).WithConfidence(confidence);
        }

        private Point3D.Builder GetPoint3DFrom(PXCMPoint3DF32 point, int confidence)
        {
            return factory.Data.Common.Point3D().From(point).WithConfidence(confidence);
        }

        public class Builder
        {
            private readonly HandsBuilder handsJointsBuilder;

            public Builder(RealSenseFactory factory)
            {
                handsJointsBuilder = new HandsBuilder(factory);
            }

            public HandsBuilder Build()
            {
                return handsJointsBuilder;
            }
        }
    }
}