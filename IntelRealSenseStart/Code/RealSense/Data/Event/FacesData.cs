using System.Collections.Generic;

namespace IntelRealSenseStart.Code.RealSense.Data.Event
{
    public class FacesData
    {
        private readonly List<FaceData> faces;

        private FacesData()
        {
            faces = new List<FaceData>();
        }

        public List<FaceData> Faces
        {
            get { return faces;  }
        }

        public class Builder
        {
            private readonly FacesData facesData;

            public Builder()
            {
                facesData = new FacesData();
            }

            public Builder WithFaceLandmarks(FaceData.Builder detectionsPoints)
            {
                facesData.faces.Add(detectionsPoints.Build());
                return this;
            }

            public FacesData Build()
            {
                return facesData;
            }
        }
    }
}