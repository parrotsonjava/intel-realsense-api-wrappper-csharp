using System;
using IntelRealSenseStart.Code.RealSense.Config.RealSense.Data;

namespace IntelRealSenseStart.Code.RealSense.Config.RealSense
{
    public class FaceIdentificationConfiguration
    {
        private const int DEFAULT_MAX_DETECTED_USERS = 10;

        private FaceIdentificationMode faceIdentificationMode;

        private String databasePath;
        private bool useExistingDatabase;

        private int maxDetectedUsers;

        private FaceIdentificationConfiguration()
        {
        }

        public FaceIdentificationMode FaceIdentificationMode
        {
            get { return faceIdentificationMode; }
        }

        public String DatabasePath
        {
            get { return databasePath; }
        }

        public bool UseExistingDatabase
        {
            get { return useExistingDatabase; }
        }

        public int MaxDetectedUsers
        {
            get { return maxDetectedUsers; }
        }

        public class Builder
        {
            private readonly FaceIdentificationConfiguration configuration;
            
            public Builder()
            {
                configuration = new FaceIdentificationConfiguration();
                WithDefaultValues();
            }

            public Builder WithDefaultValues()
            {
                return WithFaceIdentificationMode(FaceIdentificationMode.CONTINUOUS)
                    .UsingExistingDatabase(false)
                    .WithMaxDetectedUsers(DEFAULT_MAX_DETECTED_USERS);
            }

            public Builder WithFaceIdentificationMode(FaceIdentificationMode faceIdentificationMode)
            {
                configuration.faceIdentificationMode = faceIdentificationMode;
                return this;
            }

            public Builder WithDataBasePath(String databasePath)
            {
                configuration.databasePath = databasePath;
                return this;
            }

            public Builder UsingExistingDatabase(bool useExistingDatabase)
            {
                configuration.useExistingDatabase = useExistingDatabase;
                return this;
            }

            public Builder WithMaxDetectedUsers(int maxDetectedUsers)
            {
                configuration.maxDetectedUsers = maxDetectedUsers;
                return this;
            }

            public FaceIdentificationConfiguration Build()
            {
                return configuration;
            }
        }
    }
}