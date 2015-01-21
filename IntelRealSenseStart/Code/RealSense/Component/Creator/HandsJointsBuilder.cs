using System.Collections.Generic;
using IntelRealSenseStart.Code.RealSense.Data.Common;
using IntelRealSenseStart.Code.RealSense.Data.Determiner;
using IntelRealSenseStart.Code.RealSense.Data.Event;
using IntelRealSenseStart.Code.RealSense.Factory;
using IntelRealSenseStart.Code.RealSense.Helper;

namespace IntelRealSenseStart.Code.RealSense.Component.Creator
{
    public class HandsJointsBuilder
    {
        private readonly RealSenseFactory factory;

        public HandsJointsBuilder(RealSenseFactory factory)
        {
            this.factory = factory;
        }

        public HandsJointsData GetJointsData(List<HandData> handsData)
        {
            var handsJoints = factory.Data.Events.HandsJoints();
            handsData.Do(handData => handsJoints.WithFaceLandmarks(GetHandJoints(handData)));
            return handsJoints.Build();
        }

        private HandJointsData.Builder GetHandJoints(HandData handData)
        {
            var handJoints = factory.Data.Events.HandJoints();
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
            private readonly HandsJointsBuilder handsJointsBuilder;

            public Builder(RealSenseFactory factory)
            {
                handsJointsBuilder = new HandsJointsBuilder(factory);
            }

            public HandsJointsBuilder Build()
            {
                return handsJointsBuilder;
            }
        }
    }
}