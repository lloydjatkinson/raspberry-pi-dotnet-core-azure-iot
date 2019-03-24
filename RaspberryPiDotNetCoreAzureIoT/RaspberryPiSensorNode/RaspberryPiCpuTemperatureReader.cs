using Iot.Device.CpuTemperature;

namespace RaspberryPiSensorDevice
{
    public class RaspberryPiCpuTemperatureReader : ICpuTemperatureReader
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