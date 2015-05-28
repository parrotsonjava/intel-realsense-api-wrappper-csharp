using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using IntelRealSenseStart.Code.RealSense.Component.Determiner.Data;
using IntelRealSenseStart.Code.RealSense.Config.RealSense;
using IntelRealSenseStart.Code.RealSense.Config.RealSense.Data;
using IntelRealSenseStart.Code.RealSense.Data.Determiner;
using IntelRealSenseStart.Code.RealSense.Exception;
using IntelRealSenseStart.Code.RealSense.Factory;
using IntelRealSenseStart.Code.RealSense.Helper;
using IntelRealSenseStart.Code.RealSense.Provider;

namespace IntelRealSenseStart.Code.RealSense.Component.Determiner
{
    public class FaceDeterminerComponent : FrameDeterminerComponent
    {
        private const String FACE_IDENTIFICATION_DB = "FaceIdentificationDB";

        private readonly RealSenseConfiguration configuration;
        private readonly RealSenseFactory factory;
        private readonly NativeSense nativeSense;

        private PXCMFaceData faceData;

        private volatile RecognitionAction currentFaceRecognitionAction, nextFaceRecognitionAction;

        private FaceDeterminerComponent(RealSenseFactory factory, NativeSense nativeSense,
            RealSenseConfiguration configuration)
        {
            this.factory = factory;
            this.nativeSense = nativeSense;
            this.configuration = configuration;

            currentFaceRecognitionAction = RecognitionAction.DO_NOTHING;
            nextFaceRecognitionAction = RecognitionAction.DO_NOTHING;
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
            ConfigurePulse(moduleConfiguration);
            ConfigureRecognition(moduleConfiguration);
            moduleConfiguration.ApplyChanges();

            faceData = faceModule.CreateOutput();
        }

        private void ConfigurePulse(PXCMFaceConfiguration moduleConfiguration)
        {
            if (!configuration.FaceDetection.UsePulse)
            {
                return;
            }

            var pulseConfig = moduleConfiguration.QueryPulse();
            pulseConfig.properties.maxTrackedFaces = 4;
            pulseConfig.Enable();
        }

        private void ConfigureRecognition(PXCMFaceConfiguration moduleConfiguration)
        {
            if (!configuration.FaceDetection.UseIdentification)
            {
                return;
            }

            var recognitionConfig = moduleConfiguration.QueryRecognition();
            recognitionConfig.Enable();

            PXCMFaceConfiguration.RecognitionConfiguration.RecognitionStorageDesc description;
            recognitionConfig.CreateStorage(FACE_IDENTIFICATION_DB, out description);
            description.maxUsers = configuration.FaceDetection.Identification.MaxDetectedUsers;
            recognitionConfig.UseStorage(FACE_IDENTIFICATION_DB);
            recognitionConfig.SetRegistrationMode(GetRegistrationMode());

            LoadRecognitionDatabaseFromFile(recognitionConfig, configuration.FaceDetection.Identification.DatabasePath,
                configuration.FaceDetection.Identification.UseExistingDatabase);
        }

        private PXCMFaceConfiguration.RecognitionConfiguration.RecognitionRegistrationMode GetRegistrationMode()
        {
            return configuration.FaceDetection.Identification.FaceIdentificationMode ==
                   FaceIdentificationMode.CONTINUOUS
                ? PXCMFaceConfiguration.RecognitionConfiguration.RecognitionRegistrationMode
                    .REGISTRATION_MODE_CONTINUOUS
                : PXCMFaceConfiguration.RecognitionConfiguration.RecognitionRegistrationMode
                    .REGISTRATION_MODE_ON_DEMAND;
        }

        private void LoadRecognitionDatabaseFromFile(
            PXCMFaceConfiguration.RecognitionConfiguration recognitionConfig, 
            string databasePath, bool useExistingDatabase)
        {
            if (databasePath == null || !useExistingDatabase || !File.Exists(databasePath))
            {
                return;
            }

            var buffer = File.ReadAllBytes(databasePath);
            recognitionConfig.SetDatabaseBuffer(buffer);
        }

        public void Stop()
        {
            SaveFaceRecognitionDatabase();
        }

        private void SaveFaceRecognitionDatabase()
        {
            if (!configuration.FaceDetectionEnabled || !configuration.FaceDetection.UseIdentification)
            {
                return;
            }

            SaveFaceRecognitionDatabaseToFile(configuration.FaceDetection.Identification.DatabasePath);
        }

        private void SaveFaceRecognitionDatabaseToFile(String databasePath)
        {
            if (databasePath == null)
            {
                return;
            }

            var recognitionModule = faceData.QueryRecognitionModule();
            recognitionModule.QueryDatabaseSize();
            var byteSize = recognitionModule.QueryDatabaseSize();
            var buffer = new Byte[byteSize];
            recognitionModule.QueryDatabaseBuffer(buffer);

            File.WriteAllBytes(databasePath, buffer);
        }

        public void Process(DeterminerData.Builder determinerData)
        {
            faceData.Update();
            determinerData.WithFacesData(GetFacesData());
            ForgetAboutRecognitionAction();
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
                .WithLandmarks(GetLandmarkData(face))
                .WithPulse(GetPulseData(face))
                .WithFaceId(GetFaceId(face))
                .WithRecognizedId(GetRecognizedId(face));
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

        private PXCMFaceData.PulseData GetPulseData(PXCMFaceData.Face face)
        {
            if (configuration.FaceDetection.UsePulse)
            {
                return face.QueryPulse();
            }
            return null;
        }

        private int GetFaceId(PXCMFaceData.Face face)
        {
            return face.QueryUserID();
        }

        private int GetRecognizedId(PXCMFaceData.Face face)
        {
            if (!configuration.FaceDetection.UseIdentification)
            {
                return -1;
            }

            var recognitionData = face.QueryRecognition();
            if (recognitionData == null)
            {
                throw new RealSenseException("Error while querying the recognition data");
            }

            PerformRecognitionAction(recognitionData);
            return recognitionData.QueryUserID();
        }

        private void PerformRecognitionAction(PXCMFaceData.RecognitionData recognitionData)
        {
            if (currentFaceRecognitionAction == RecognitionAction.REGISTER)
            {
                recognitionData.RegisterUser();
            }
            else if (currentFaceRecognitionAction == RecognitionAction.UNREGISTER && recognitionData.IsRegistered())
            {
                recognitionData.UnregisterUser();
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        private void ForgetAboutRecognitionAction()
        {
            currentFaceRecognitionAction = nextFaceRecognitionAction;
            nextFaceRecognitionAction = RecognitionAction.DO_NOTHING;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void RegisterFaces()
        {
            CheckRecognitionForAction();
            nextFaceRecognitionAction = RecognitionAction.REGISTER;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UnregisterFaces()
        {
            CheckRecognitionForAction();
            nextFaceRecognitionAction = RecognitionAction.UNREGISTER;
        }

        private void CheckRecognitionForAction()
        {
            if (!configuration.FaceDetectionEnabled ||
                !configuration.FaceDetection.UseIdentification)
            {
                throw new RealSenseException("Face detection or face identifation was not configured");
            }
            if (configuration.FaceDetection.Identification.FaceIdentificationMode != FaceIdentificationMode.ON_DEMAND)
            {
                throw new RealSenseException(
                    "Face recognition works automatically unless the face identifaction mode is set to on demand");
            }
        }

        public class Builder
        {
            private RealSenseFactory factory;
            private NativeSense nativeSense;
            private RealSenseConfiguration configuration;

            public Builder WithFactory(RealSenseFactory factory)
            {
                this.factory = factory;
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
                    "The factory must be set in order to create the hands determiner component");
                nativeSense.Check(Preconditions.IsNotNull,
                    "The RealSense manager must be set in order to create the hands determiner component");
                configuration.Check(Preconditions.IsNotNull,
                    "The RealSense configuration must be set in order to create the hands determiner component");

                return new FaceDeterminerComponent(factory, nativeSense, configuration);
            }
        }
    }
}