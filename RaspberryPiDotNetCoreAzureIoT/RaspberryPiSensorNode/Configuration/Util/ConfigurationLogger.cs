using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Newtonsoft.Json;

using System;

namespace RaspberryPiSensorNode.Configuration.Util
{
    public class ConfigurationLogger : IConfigurationLogger
    {
        private readonly ILogger<ConfigurationLogger> _logger;
        private readonly CpuTemperatureMonitorConfiguration _cpuTemperatureMonitorConfiguration;
        private readonly AzureIoTHubConfiguration _azureIoTHubConfiguration;

        public ConfigurationLogger(
            ILogger<ConfigurationLogger> logger,
            IOptions<CpuTemperatureMonitorConfiguration> cpuTemperatureMonitorConfiguration,
            IOptions<AzureIoTHubConfiguration> azureIoTHubConfiguration)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _cpuTemperatureMonitorConfiguration = cpuTemperatureMonitorConfiguration.Value ?? throw new ArgumentNullException(nameof(cpuTemperatureMonitorConfiguration));
            _azureIoTHubConfiguration = azureIoTHubConfiguration.Value ?? throw new ArgumentNullException(nameof(azureIoTHubConfiguration));
        }

        public void LogConfiguration()
        {
            _logger.LogInformation($"CPU temperature monitor configuration:{Environment.NewLine}{JsonConvert.SerializeObject(_cpuTemperatureMonitorConfiguration, Formatting.Indented)}");
            _logger.LogInformation($"Azure IoT Hub configuration:{Environment.NewLine}{JsonConvert.SerializeObject(_azureIoTHubConfiguration, Formatting.Indented)}");
        }
    }
}