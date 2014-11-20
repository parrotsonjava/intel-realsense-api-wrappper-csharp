using System.Collections.Generic;

namespace IntelRealSenseStart.Code.RealSense.Data
{
    public class HandData
    {
        private PXCMHandData.BodySideType bodySide;
        private Dictionary<PXCMHandData.JointType, PXCMHandData.JointData> joints;
        private PXCMImage segmentationImage;

        private HandData()
        {
        }

        public PXCMHandData.BodySideType BodySide
        {
            get { return bodySide;  }
        }

        public Dictionary<PXCMHandData.JointType, PXCMHandData.JointData> Joints
        {
            get { return joints ?? new Dictionary<PXCMHandData.JointType, PXCMHandData.JointData>(); }
        }

        public bool HasJoints
        {
            get { return joints != null; }
        }

        public PXCMImage SegmentationImage
        {
            get { return segmentationImage; }
        }

        public bool HasSegmentationImage
        {
            get { return segmentationImage != null;  }
        }

        public class Builder
        {
            private readonly HandData frameEvent;

            public Builder()
            {
                frameEvent = new HandData();
            }
            public Builder WithBodySide(PXCMHandData.BodySideType bodySide)
            {
                frameEvent.bodySide = bodySide;
                return this;
            }

            public Builder WithJoints(Dictionary<PXCMHandData.JointType, PXCMHandData.JointData> joints)
            {
                frameEvent.joints = joints;
                return this;
            }

            public Builder WithSegmentationImage(PXCMImage segmentationImage)
            {
                frameEvent.segmentationImage = segmentationImage;
                return this;
            }

            public HandData Build()
            {
                return frameEvent;
            }
        }

    }
}