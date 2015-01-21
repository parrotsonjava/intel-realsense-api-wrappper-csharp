using System.Collections.Generic;
using System.Linq;
using IntelRealSenseStart.Code.RealSense.Config.RealSense;
using IntelRealSenseStart.Code.RealSense.Data.Determiner;
using IntelRealSenseStart.Code.RealSense.Factory;
using IntelRealSenseStart.Code.RealSense.Helper;
using IntelRealSenseStart.Code.RealSense.Provider;

namespace IntelRealSenseStart.Code.RealSense.Component.Determiner
{
    public class HandsDeterminerComponent : DeterminerComponent
    {
        private readonly RealSenseConfiguration configuration;

        private readonly RealSenseFactory factory;
        private readonly SenseManagerProvider senseManagerProvider;

        private PXCMHandData handData;

        private HandsDeterminerComponent(RealSenseFactory factory, SenseManagerProvider senseManagerProvider, RealSenseConfiguration configuration)
        {
            this.factory = factory;
            this.senseManagerProvider = senseManagerProvider;
            this.configuration = configuration;
        }

        public bool ShouldBeStarted
        {
            get { return configuration.HandsDetectionEnabled; }
        }

        public void EnableFeatures()
        {
            if (configuration.HandsDetectionEnabled)
            {
                senseManagerProvider.SenseManager.EnableHand();
            }
        }
        
        public void Configure()
        {
            PXCMHandModule handModule = senseManagerProvider.SenseManager.QueryHand();
            handData = handModule.CreateOutput();
            PXCMHandConfiguration handConfiguration = handModule.CreateActiveConfiguration();

            ConfigureHandOptions(handConfiguration);

            handConfiguration.ApplyChanges();
            handConfiguration.Update();
        }

        private void ConfigureHandOptions(PXCMHandConfiguration handConfiguration)
        {
            // TODO no more hardcoding
            handConfiguration.DisableAllGestures();
            if (configuration.HandsDetection.SegmentationImageEnabled)
            {
                handConfiguration.EnableSegmentationImage(true);
            }
        }

        public void Process(DeterminerData.Builder determinerData)
        {
            PXCMCapture.Sample realSenseSample = senseManagerProvider.SenseManager.QuerySample();
            PXCMCapture.Sample handSample = senseManagerProvider.SenseManager.QueryHandSample();

            if (realSenseSample != null && handSample != null)
            {
                handData.Update();
                determinerData.WithHandsData(GetHandsData());
            }
        }

        private HandsData.Builder GetHandsData()
        {
            return factory.Data.Determiner.Hands().WithHands(
                GetIndividualHandSamples().Select(GetIndividualHandData));
        }

        private IEnumerable<PXCMHandData.IHand> GetIndividualHandSamples()
        {
            return 0.To(handData.QueryNumberOfHands()).ToArray().Select(index =>
            {
                PXCMHandData.IHand oneHandData;
                handData.QueryHandData(PXCMHandData.AccessOrderType.ACCESS_ORDER_BY_TIME, index, out oneHandData);
                return oneHandData;
            }).Where(oneHandData => oneHandData != null);
        }

        private HandData.Builder GetIndividualHandData(PXCMHandData.IHand individualHandSample)
        {
            return factory.Data.Determiner.Hand()
                .WithBodySide(GetUserId(individualHandSample))
                .WithJoints(GetJointData(individualHandSample))
                .WithSegmentationImage(GetSegmentationImage(individualHandSample));
        }

        private PXCMHandData.BodySideType GetUserId(PXCMHandData.IHand individualHandSample)
        {
            return individualHandSample.QueryBodySide();
        }

        private Dictionary<PXCMHandData.JointType, PXCMHandData.JointData> GetJointData(
            PXCMHandData.IHand individualHandSample)
        {
            return 0.To(21).ToArray().ToDictionary(
                index => (PXCMHandData.JointType) index,
                index =>
                {
                    PXCMHandData.JointData jointData;
                    individualHandSample.QueryTrackedJoint((PXCMHandData.JointType) index, out jointData);
                    return jointData;
                });
        }

        private PXCMImage GetSegmentationImage(PXCMHandData.IHand individualHandSample)
        {
            if (configuration.HandsDetection.SegmentationImageEnabled)
            {
                PXCMImage segmentationImage;
                individualHandSample.QuerySegmentationImage(out segmentationImage);
                return segmentationImage;
            }

            return null;
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

            public HandsDeterminerComponent Build()
            {
                factory.Check(Preconditions.IsNotNull,
                    "The factory must be set in order to create the hands determiner component");
                senseManagerProvider.Check(Preconditions.IsNotNull,
                    "The RealSense manager must be set in order to create the hands determiner component");
                configuration.Check(Preconditions.IsNotNull,
                    "The RealSense configuration must be set in order to create the hands determiner component");

                return new HandsDeterminerComponent(factory, senseManagerProvider, configuration);
            }
        }
    }
}