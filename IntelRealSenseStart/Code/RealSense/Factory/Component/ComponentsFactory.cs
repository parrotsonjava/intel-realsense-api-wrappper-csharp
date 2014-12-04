using IntelRealSenseStart.Code.RealSense.Config;

namespace IntelRealSenseStart.Code.RealSense.Factory.Component
{
    public class ComponentsFactory
    {
        private readonly CreatorComponentsFactory creatorComponentsFactory;
        private readonly DeterminerComponentsFactory determinerComponentsFactory;
        private readonly PropertiesComponentsFactory _propertiesComponentsFactory;

        public ComponentsFactory()
        {
            creatorComponentsFactory = new CreatorComponentsFactory();
            determinerComponentsFactory = new DeterminerComponentsFactory();
            _propertiesComponentsFactory = new PropertiesComponentsFactory();
        }

        public CreatorComponentsFactory Creator
        {
            get { return creatorComponentsFactory; }
        }

        public DeterminerComponentsFactory Determiner
        {
            get { return determinerComponentsFactory;  }
        }

        public PropertiesComponentsFactory Properties
        {
            get { return _propertiesComponentsFactory; }
        }
    }
}