using System;
using System.IO;
using System.Runtime.CompilerServices;
using IntelRealSenseStart.Code.RealSense.Component.Determiner.Data;
using IntelRealSenseStart.Code.RealSense.Config.RealSense;
using IntelRealSenseStart.Code.RealSense.Config.RealSense.Data;
using IntelRealSenseStart.Code.RealSense.Data.Determiner;
using IntelRealSenseStart.Code.RealSense.Exception;
using IntelRealSenseStart.Code.RealSense.Helper;

namespace IntelRealSenseStart.Code.RealSense.Component.Determiner.Face
{
    public class FaceRecognitionDeterminerComponent : FaceComponent
    {
        private const String FACE_IDENTIFICATION_DB = "FaceIdentificationDB";

        private readonly RealSenseConfiguration configuration;

        private volatile RecognitionAction currentFaceRecognitionAction, nextFaceRecognitionAction;

        private FaceRecognitionDeterminerComponent(RealSenseConfiguration configuration)
        {
            this.configuration = configuration;

            currentFaceRecognitionAction = RecognitionAction.DO_NOTHING;
            nextFaceRecognitionAction = RecognitionAction.DO_NOTHING;
        }

        public void EnableFeatures()
        {
            // Nothing to do
        }

        public void Configure(PXCMFaceConfiguration moduleConfiguration)
        {
            ConfigureRecognition(moduleConfiguration);
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

        private void LoadRecognitionDatabaseFromFile(PXCMFaceConfiguration.RecognitionConfiguration recognitionConfig,
            String databasePath, bool useExistingDatabase)
        {
            if (databasePath == null || !useExistingDatabase || !File.Exists(databasePath))
            {
                return;
            }

            var buffer = File.ReadAllBytes(databasePath);
            recognitionConfig.SetDatabaseBuffer(buffer);
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

        public void Process(int index, PXCMFaceData.Face face, FaceDeterminerData.Builder faceDeterminerData)
        {
            faceDeterminerData
                .WithFaceId(GetFaceId(face))
                .WithRecognizedId(GetRecognizedId(face));
            ForgetAboutRecognitionAction();
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

        public void Stop(PXCMFaceData faceData)
        {
            SaveFaceRecognitionDatabase(faceData);
        }

        private void SaveFaceRecognitionDatabase(PXCMFaceData faceData)
        {
            if (!configuration.FaceDetectionEnabled || !configuration.FaceDetection.UseIdentification)
            {
                return;
            }

            SaveFaceRecognitionDatabaseToFile(faceData, configuration.FaceDetection.Identification.DatabasePath);
        }

        private void SaveFaceRecognitionDatabaseToFile(PXCMFaceData faceData, String databasePath)
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

        public class Builder
        {
            private RealSenseConfiguration configuration;

            public Builder WithConfiguration(RealSenseConfiguration configuration)
            {
                this.configuration = configuration;
                return this;
            }

            public FaceRecognitionDeterminerComponent Build()
            {
                configuration.Check(Preconditions.IsNotNull,
                    "The RealSense configuration must be set in order to create the face recognition determiner component");

                return new FaceRecognitionDeterminerComponent(configuration);
            }
        }
    }
}