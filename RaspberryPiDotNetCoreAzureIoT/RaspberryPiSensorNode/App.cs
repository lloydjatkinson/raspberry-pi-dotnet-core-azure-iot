using System;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using RaspberryPiSensorDevice.Configuration;

namespace RaspberryPiSensorDevice
{
    public class App
    {
        private readonly ILogger<App> _logger;
        private readonly AzureIoTHubConfiguration _azureIoTHubConfiguration;

        public App(ILogger<App> logger, IOptions<AzureIoTHubConfiguration> azureIoTHubConfiguration)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _azureIoTHubConfiguration = azureIoTHubConfiguration.Value ?? throw new ArgumentNullException(nameof(azureIoTHubConfiguration));
        }

        public async Task Run()
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
        }
    }
}