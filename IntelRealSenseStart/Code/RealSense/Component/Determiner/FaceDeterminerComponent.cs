using System.Collections.Generic;
using System.Linq;
using IntelRealSenseStart.Code.RealSense.Component.Determiner.Face;
using IntelRealSenseStart.Code.RealSense.Config.RealSense;
using IntelRealSenseStart.Code.RealSense.Data.Determiner;
using IntelRealSenseStart.Code.RealSense.Factory;
using IntelRealSenseStart.Code.RealSense.Helper;
using IntelRealSenseStart.Code.RealSense.Provider;

namespace IntelRealSenseStart.Code.RealSense.Component.Determiner
{
    public class FaceDeterminerComponent : FrameDeterminerComponent
    {
        private readonly IEnumerable<FaceComponent> faceComponents;

        private readonly RealSenseConfiguration configuration;
        private readonly RealSenseFactory factory;
        private readonly NativeSense nativeSense;

        private PXCMFaceData faceData;
        
        private FaceDeterminerComponent(IEnumerable<FaceComponent> faceComponents,
            RealSenseFactory factory, NativeSense nativeSense, RealSenseConfiguration configuration)
        {
            this.faceComponents = faceComponents;
            this.factory = factory;
            this.nativeSense = nativeSense;
            this.configuration = configuration;
        }

        public bool ShouldBeStarted
        {
            get { return configuration.FaceDetectionEnabled; }
        }

        public void EnableFeatures()
        {
            if (configuration.FaceDetectionEnabled)
            {
                nativeSense.SenseManager.EnableFace();
            }
        }

        public void Configure()
        {
            PXCMFaceModule faceModule = nativeSense.SenseManager.QueryFace();
            PXCMFaceConfiguration moduleConfiguration = faceModule.CreateActiveConfiguration();

            moduleConfiguration.SetTrackingMode(PXCMFaceConfiguration.TrackingModeType.FACE_MODE_COLOR_PLUS_DEPTH);
            moduleConfiguration.strategy = PXCMFaceConfiguration.TrackingStrategyType.STRATEGY_RIGHT_TO_LEFT;

            ConfigureComponents(moduleConfiguration);
            
            moduleConfiguration.ApplyChanges();
            faceData = faceModule.CreateOutput();
        }

        private void ConfigureComponents(PXCMFaceConfiguration moduleConfiguration)
        {
            faceComponents.Do(faceComponent => faceComponent.Configure(moduleConfiguration));
        }

        public void Process(DeterminerData.Builder determinerData)
        {
            faceData.Update();
            determinerData.WithFacesData(GetFacesData());
        }

        private FacesData.Builder GetFacesData()
        {
            return factory.Data.Determiner.Faces().WithFaces(
                GetIndividualFaces().Select(GetIndividualFaceData));
        }

        private IEnumerable<PXCMFaceData.Face> GetIndividualFaces()
        {
            return 0.To(faceData.QueryNumberOfDetectedFaces()).ToArray()
                .Select(index => faceData.QueryFaceByIndex(index))
                .Where(face => face != null);
        }

        private FaceDeterminerData.Builder GetIndividualFaceData(PXCMFaceData.Face face)
        {
            var faceDeterminerData = factory.Data.Determiner.Face();
            ProcessComponents(face, faceDeterminerData);
            return faceDeterminerData;
        }

        private void ProcessComponents(PXCMFaceData.Face face, FaceDeterminerData.Builder faceDeterminerData)
        {
            faceComponents.Do(faceComponent => faceComponent.Process(face, faceDeterminerData));
        }

        public void Stop()
        {
            faceComponents.Do(faceComponent => faceComponent.Stop(faceData));
        }

        public void RegisterFaces()
        {
            GetFaceComponentOfType<FaceRecognitionDeterminerComponent>().RegisterFaces();
        }

        public void UnregisterFaces()
        {
            GetFaceComponentOfType<FaceRecognitionDeterminerComponent>().UnregisterFaces();
        }

        private T GetFaceComponentOfType<T>() where T : FaceComponent
        {
            return (T) faceComponents.First(component => component.GetType() == typeof (T));
        }

        public class Builder
        {
            private readonly List<FaceComponent> faceComponents;

            private RealSenseFactory factory;
            private NativeSense nativeSense;
            private RealSenseConfiguration configuration;

            public Builder()
            {
                faceComponents = new List<FaceComponent>();
            }

            public Builder WithFactory(RealSenseFactory factory)
            {
                this.factory = factory;
                return this;
            }

            public Builder WithFaceComponent(FaceComponent faceComponent)
            {
                faceComponents.Add(faceComponent);
                return this;
            }

            public Builder WithNativeSense(NativeSense nativeSense)
            {
                this.nativeSense = nativeSense;
                return this;
            }

            public Builder WithConfiguration(RealSenseConfiguration configuration)
            {
                this.configuration = configuration;
                return this;
            }

            public FaceDeterminerComponent Build()
            {
                factory.Check(Preconditions.IsNotNull,
                    "The factory must be set in order to create the face determiner component");
                nativeSense.Check(Preconditions.IsNotNull,
                    "The RealSense manager must be set in order to create the face determiner component");
                configuration.Check(Preconditions.IsNotNull,
                    "The RealSense configuration must be set in order to create the face determiner component");

                return new FaceDeterminerComponent(faceComponents, factory, nativeSense, configuration);
            }
        }
    }
}