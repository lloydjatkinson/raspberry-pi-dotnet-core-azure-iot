using System;

namespace RaspberryPiSensorNode.Configuration
{
    public class CpuTemperatureMonitorConfiguration
    {
        public TimeSpan ReadInterval { get; set; } = TimeSpan.FromMinutes(1);
    }
}