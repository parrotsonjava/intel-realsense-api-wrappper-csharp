using System;
using System.Collections.Generic;
using System.Linq;
using IntelRealSenseStart.Code.RealSense.Config.RealSense;
using IntelRealSenseStart.Code.RealSense.Data.Determiner;
using IntelRealSenseStart.Code.RealSense.Factory;
using IntelRealSenseStart.Code.RealSense.Helper;
using IntelRealSenseStart.Code.RealSense.Provider;

namespace IntelRealSenseStart.Code.RealSense.Component.Determiner
{
    public class FaceDeterminerComponent : DeterminerComponent
    {
        private readonly RealSenseConfiguration configuration;

        private readonly RealSenseFactory factory;
        private readonly SenseManagerProvider senseManagerProvider;

        private PXCMFaceData faceData;

        private FaceDeterminerComponent(RealSenseFactory factory, SenseManagerProvider senseManagerProvider, RealSenseConfiguration configuration)
        {
            this.factory = factory;
            this.senseManagerProvider = senseManagerProvider;
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
                senseManagerProvider.SenseManager.EnableFace();
            }
        }

        public void Configure()
        {
            PXCMFaceModule faceModule = senseManagerProvider.SenseManager.QueryFace();
            PXCMFaceConfiguration moduleConfiguration = faceModule.CreateActiveConfiguration();

            moduleConfiguration.SetTrackingMode(PXCMFaceConfiguration.TrackingModeType.FACE_MODE_COLOR_PLUS_DEPTH);
            moduleConfiguration.strategy = PXCMFaceConfiguration.TrackingStrategyType.STRATEGY_RIGHT_TO_LEFT;
            moduleConfiguration.ApplyChanges();

            faceData = faceModule.CreateOutput();
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

        private FaceData.Builder GetIndividualFaceData(PXCMFaceData.Face face)
        {
            return factory.Data.Determiner.Face()
                .WithLandmarks(GetLandmarkData(face));
        }

        private PXCMFaceData.LandmarkPoint[] GetLandmarkData(PXCMFaceData.Face face)
        {
            if (configuration.FaceDetection.UseLandmarks)
            {
                var landMarks = face.QueryLandmarks();
                return GetLandmarkPoints(landMarks);
            }
            return null;
        }

        private static PXCMFaceData.LandmarkPoint[] GetLandmarkPoints(PXCMFaceData.LandmarksData landMarks)
        {
            if (landMarks == null)
            {
                return null;
            }

            PXCMFaceData.LandmarkPoint[] points;
            landMarks.QueryPoints(out points);
            return points;
        }

        public class Builder
        {
            private RealSenseFactory factory;
            private SenseManagerProvider senseManagerProvider;
            private RealSenseConfiguration configuration;

            public Builder WithFactory(RealSenseFactory factory)
            {
                this.factory = factory;
                return this;
            }

            public Builder WithManager(SenseManagerProvider senseManagerProvider)
            {
                this.senseManagerProvider = senseManagerProvider;
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
                    "The factory must be set in order to create the hands determiner component");
                senseManagerProvider.Check(Preconditions.IsNotNull,
                    "The RealSense manager must be set in order to create the hands determiner component");
                configuration.Check(Preconditions.IsNotNull,
                    "The RealSense configuration must be set in order to create the hands determiner component");

                return new FaceDeterminerComponent(factory, senseManagerProvider, configuration);
            }
        }
    }
}