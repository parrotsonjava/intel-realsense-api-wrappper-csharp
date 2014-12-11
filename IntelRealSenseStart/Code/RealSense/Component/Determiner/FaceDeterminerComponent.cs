﻿using System.Collections.Generic;
using System.Linq;
using IntelRealSenseStart.Code.RealSense.Config.RealSense;
using IntelRealSenseStart.Code.RealSense.Data.Determiner;
using IntelRealSenseStart.Code.RealSense.Event;
using IntelRealSenseStart.Code.RealSense.Factory;
using IntelRealSenseStart.Code.RealSense.Helper;

namespace IntelRealSenseStart.Code.RealSense.Component.Determiner
{
    public class FaceDeterminerComponent : DeterminerComponent
    {
        private readonly Configuration configuration;

        private readonly RealSenseFactory factory;
        private readonly PXCMSenseManager manager;

        private PXCMFaceData faceData;

        private FaceDeterminerComponent(RealSenseFactory factory, PXCMSenseManager manager, Configuration configuration)
        {
            this.factory = factory;
            this.manager = manager;
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
                manager.EnableFace();
            }
        }

        public void Configure()
        {
            PXCMFaceModule faceModule = manager.QueryFace();
            PXCMFaceConfiguration moduleConfiguration = faceModule.CreateActiveConfiguration();

            moduleConfiguration.SetTrackingMode(PXCMFaceConfiguration.TrackingModeType.FACE_MODE_COLOR_PLUS_DEPTH);
            moduleConfiguration.strategy = PXCMFaceConfiguration.TrackingStrategyType.STRATEGY_RIGHT_TO_LEFT;

            faceData = faceModule.CreateOutput();
        }

        public void Process(FrameEventArgs.Builder frameEvent)
        {
            faceData.Update();
            frameEvent.WithFacesData(GetFacesData());
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
                PXCMFaceData.LandmarkPoint[] points;
                landMarks.QueryPoints(out points);
                return points;
            }
            return null;
        }

        public class Builder
        {
            private RealSenseFactory factory;
            private PXCMSenseManager manager;
            private Configuration configuration;

            public Builder WithFactory(RealSenseFactory factory)
            {
                this.factory = factory;
                return this;
            }

            public Builder WithManager(PXCMSenseManager manager)
            {
                this.manager = manager;
                return this;
            }

            public Builder WithConfiguration(Configuration configuration)
            {
                this.configuration = configuration;
                return this;
            }

            public FaceDeterminerComponent Build()
            {
                factory.CheckState(Preconditions.IsNotNull,
                    "The factory must be set in order to create the hands determiner component");
                manager.CheckState(Preconditions.IsNotNull,
                    "The RealSense manager must be set in order to create the hands determiner component");
                configuration.CheckState(Preconditions.IsNotNull,
                    "The RealSense configuration must be set in order to create the hands determiner component");

                return new FaceDeterminerComponent(factory, manager, configuration);
            }
        }
    }
}