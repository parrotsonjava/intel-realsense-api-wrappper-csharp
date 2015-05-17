namespace IntelRealSenseStart.Code.RealSense.Factory.Component
{
    public class ComponentsFactory
    {
        private readonly CreatorComponentsFactory creatorComponentsFactory;
        private readonly DeterminerComponentsFactory determinerComponentsFactory;
        private readonly OutputComponentsFactory outputComponentsFactory;
        private readonly PropertiesComponentsFactory propertiesComponentsFactory;

        public ComponentsFactory(RealSenseFactory realSenseFactory)
        {
            creatorComponentsFactory = new CreatorComponentsFactory(realSenseFactory);
            determinerComponentsFactory = new DeterminerComponentsFactory();
            outputComponentsFactory = new OutputComponentsFactory();
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
        public OutputComponentsFactory Output
        {
            get { return outputComponentsFactory; }
        }

        public PropertiesComponentsFactory Properties
        {
            get { return propertiesComponentsFactory; }
        }
    }
}