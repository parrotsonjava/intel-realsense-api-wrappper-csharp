namespace IntelRealSenseStart.Code.RealSense.Factory.Data
{
    public class DataFactory
    {
        private readonly DeterminerDataFactory determinerDataFactory;

        public DataFactory()
        {
            determinerDataFactory = new DeterminerDataFactory();
        }

        public DeterminerDataFactory Determiner
        {
            get { return determinerDataFactory; }
        }
    }
}