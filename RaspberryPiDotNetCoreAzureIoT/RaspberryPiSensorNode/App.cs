using Microsoft.Extensions.Logging;

using RaspberryPiSensorNode.Configuration.Util;
using RaspberryPiSensorNode.Temperature;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace RaspberryPiSensorNode
{
    internal class App
    {
        private readonly ILogger<App> _logger;
        private readonly ICpuTemperatureMonitor _cpuTemperatureMonitor;
        private readonly IConfigurationLogger _configurationLogger;

        public App(ILogger<App> logger, ICpuTemperatureMonitor cpuTemperatureMonitor, IConfigurationLogger configurationLogger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _cpuTemperatureMonitor = cpuTemperatureMonitor;
            _configurationLogger = configurationLogger;
        }

        public async Task Run(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Raspberry Pi Sensor Node started.");
            //_logger.LogInformation($"Running on .NET Core {Environment.Version}"); // Commented out for now - .NET Core 3 finally fixes this, returns 4.xx on .NET Core 2.2.
            _configurationLogger.LogConfiguration();

            await _cpuTemperatureMonitor.Run(cancellationToken);
        }
    }
}