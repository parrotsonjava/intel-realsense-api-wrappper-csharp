using System.Collections.Generic;
using System.Linq;

namespace IntelRealSenseStart.Code.RealSense.Data.Determiner
{
    public class FacesData
    {
        private readonly List<FaceDeterminerData> faces;

        public FacesData()
        {
            faces = new List<FaceDeterminerData>();
        }

        public List<FaceDeterminerData> Faces
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

            public Builder WithFace(FaceDeterminerData.Builder faceData)
            {
                facesData.Faces.Add(faceData.Build());
                return this;
            }

            public Builder WithFaces(IEnumerable<FaceDeterminerData.Builder> faceData)
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