namespace RaspberryPiSensorNode.Temperature
{
    public interface ITemperatureReader
    {
        TemperatureReading Read();
    }
}