namespace IntelRealSenseStart.Code.RealSense.Component.Property
{
    public interface PropertiesComponent<in PROPERTIES_BUILDER>
    {
        void UpdateProperties(PROPERTIES_BUILDER videoProperties);
    }
}
