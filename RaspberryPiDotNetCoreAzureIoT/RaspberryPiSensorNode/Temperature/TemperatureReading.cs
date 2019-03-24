namespace RaspberryPiSensorNode.Temperature
{
    public class TemperatureReading
    {
        public bool CouldRead { get; }

        public double Celcius { get; }

        public TemperatureReading(bool couldRead, double celcius)
        {
            CouldRead = couldRead;
            Celcius = celcius;
        }
    }
}
