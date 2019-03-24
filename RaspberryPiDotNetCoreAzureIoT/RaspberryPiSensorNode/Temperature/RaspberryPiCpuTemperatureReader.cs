using Iot.Device.CpuTemperature;

namespace RaspberryPiSensorNode.Temperature
{
    public class RaspberryPiCpuTemperatureReader : ITemperatureReader
    {
        private readonly CpuTemperature _cpuTemperature;

        public RaspberryPiCpuTemperatureReader()
        {
            _cpuTemperature = new CpuTemperature();
        }

        public TemperatureReading Read()
        {
            return new TemperatureReading(_cpuTemperature.IsAvailable, _cpuTemperature.Temperature.Celsius);
        }
    }
}