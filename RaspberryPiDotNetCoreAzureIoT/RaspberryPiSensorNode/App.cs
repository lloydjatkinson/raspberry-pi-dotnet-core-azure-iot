using System;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using RaspberryPiSensorNode.Configuration;
using RaspberryPiSensorNode.Temperature;

namespace RaspberryPiSensorNode
{
    internal class App
    {
        private readonly ILogger<App> _logger;
        private readonly ICpuTemperatureMonitor _cpuTemperatureMonitor;
        private readonly AzureIoTHubConfiguration _azureIoTHubConfiguration;

        public App(ILogger<App> logger, IOptions<AzureIoTHubConfiguration> azureIoTHubConfiguration, ICpuTemperatureMonitor cpuTemperatureMonitor)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _azureIoTHubConfiguration = azureIoTHubConfiguration.Value ?? throw new ArgumentNullException(nameof(azureIoTHubConfiguration));
            _cpuTemperatureMonitor = cpuTemperatureMonitor;
        }

        public async Task Run(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Raspberry Pi Sensor Node started.");
            if (!string.IsNullOrWhiteSpace(_azureIoTHubConfiguration.ConnectionString))
            {
                _logger.LogInformation("Azure IoT Hub connection read from application configuration.");
            }
            else
            {
                _logger.LogWarning("Azure IoT Hub connection string is not in the application configuration.");
            }

            await _cpuTemperatureMonitor.Run(cancellationToken);
        }
    }
}