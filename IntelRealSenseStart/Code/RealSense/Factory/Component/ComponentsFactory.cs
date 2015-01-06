namespace IntelRealSenseStart.Code.RealSense.Factory.Component
{
    public class ComponentsFactory
    {
        private readonly CreatorComponentsFactory creatorComponentsFactory;
        private readonly DeterminerComponentsFactory determinerComponentsFactory;
        private readonly PropertiesComponentsFactory propertiesComponentsFactory;

        public ComponentsFactory(RealSenseFactory realSenseFactory)
        {
            creatorComponentsFactory = new CreatorComponentsFactory(realSenseFactory);
            determinerComponentsFactory = new DeterminerComponentsFactory();
            propertiesComponentsFactory = new PropertiesComponentsFactory();
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
            get { return propertiesComponentsFactory; }
        }
    }
}