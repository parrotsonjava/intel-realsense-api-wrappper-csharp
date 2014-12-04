using IntelRealSenseStart.Code.RealSense.Config;

namespace IntelRealSenseStart.Code.RealSense.Factory.Component
{
    public class ComponentsFactory
    {
        private readonly CreatorComponentsFactory creatorComponentsFactory;
        private readonly DeterminerComponentsFactory determinerComponentsFactory;
        private readonly PropertyComponentsFactory propertyComponentsFactory;

        public ComponentsFactory()
        {
            creatorComponentsFactory = new CreatorComponentsFactory();
            determinerComponentsFactory = new DeterminerComponentsFactory();
            propertyComponentsFactory = new PropertyComponentsFactory();
        }

        public CreatorComponentsFactory Creator
        {
            get { return creatorComponentsFactory; }
        }

        public DeterminerComponentsFactory Determiner
        {
            get { return determinerComponentsFactory;  }
        }

        public PropertyComponentsFactory Property
        {
            get { return propertyComponentsFactory; }
        }
    }
}