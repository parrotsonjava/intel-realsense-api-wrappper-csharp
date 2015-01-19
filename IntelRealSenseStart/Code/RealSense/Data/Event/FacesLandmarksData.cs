using System.Collections.Generic;

namespace IntelRealSenseStart.Code.RealSense.Data.Event
{
    public class FacesLandmarksData
    {
        private readonly List<FaceLandmarksData> faces;

        private FacesLandmarksData()
        {
            faces = new List<FaceLandmarksData>();
        }

        public List<FaceLandmarksData> Faces
        {
            get { return faces;  }
        }

        public class Builder
        {
            private readonly FacesLandmarksData facesLandmarksData;

            public Builder()
            {
                facesLandmarksData = new FacesLandmarksData();
            }

            public Builder WithFaceLandmarks(FaceLandmarksData.Builder detectionsPoints)
            {
                facesLandmarksData.faces.Add(detectionsPoints.Build());
                return this;
            }

            public FacesLandmarksData Build()
            {
                return facesLandmarksData;
            }
        }
    }
}