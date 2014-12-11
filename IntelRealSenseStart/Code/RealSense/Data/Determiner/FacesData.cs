using System.Collections.Generic;
using System.Linq;

namespace IntelRealSenseStart.Code.RealSense.Data.Determiner
{
    public class FacesData
    {
        private readonly List<FaceData> faces;

        public FacesData()
        {
            faces = new List<FaceData>();
        }

        public List<FaceData> Faces
        {
            get { return faces; }
        }

        public class Builder
        {
            private readonly FacesData facesData;

            public Builder()
            {
                facesData = new FacesData();
            }

            public Builder WithFace(FaceData.Builder faceData)
            {
                facesData.Faces.Add(faceData.Build());
                return this;
            }

            public Builder WithFaces(IEnumerable<FaceData.Builder> faceData)
            {
                facesData.Faces.AddRange(faceData.Select(faceBuilder => faceBuilder.Build()));
                return this;
            }

            public FacesData Build()
            {
                return facesData;
            }
        }
    }
}