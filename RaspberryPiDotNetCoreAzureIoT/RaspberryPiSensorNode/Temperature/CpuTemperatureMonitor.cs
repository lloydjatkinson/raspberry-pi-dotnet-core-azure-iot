using Microsoft.Azure.Devices.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

using RaspberryPiSensorNode.Configuration;

using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RaspberryPiSensorNode.Temperature
{
    public class CpuTemperatureMonitor : ICpuTemperatureMonitor
    {
        private readonly ILogger<CpuTemperatureMonitor> _logger;
        private readonly IOptions<AzureIoTHubConfiguration> _azureIoTHubConnectionConfiguration; //TODO: Look into removing dependency on IOptions in here.
        private readonly IOptions<CpuTemperatureMonitorConfiguration> _cpuTemperatureMonitorConfiguration;
        private readonly ITemperatureReader _temperatureReader;
        private readonly DeviceClient _deviceClient;

        public CpuTemperatureMonitor(
            ILogger<CpuTemperatureMonitor> logger,
            IOptions<AzureIoTHubConfiguration> azureIoTHubConnectionConfiguration,
            IOptions<CpuTemperatureMonitorConfiguration> cpuTemperatureMonitorConfiguration,
            ITemperatureReader temperatureReader
        )
        {
            _logger = logger;
            _azureIoTHubConnectionConfiguration = azureIoTHubConnectionConfiguration;
            _cpuTemperatureMonitorConfiguration = cpuTemperatureMonitorConfiguration;
            _temperatureReader = temperatureReader;
            _deviceClient = DeviceClient.CreateFromConnectionString(_azureIoTHubConnectionConfiguration.Value.ConnectionString, TransportType.Mqtt);
        }

        public async Task Run(CancellationToken cancellationToken)
        {
            var readInterval = _cpuTemperatureMonitorConfiguration.Value.ReadInterval;

            _logger.LogInformation($"Starting to monitor CPU temperature. Will read every {readInterval}.");

            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var reading = _temperatureReader.Read();
                    var test = new TemperatureReading(true, 12.34d);

                    var message = new Message(
                        Encoding.ASCII.GetBytes(
                            JsonConvert.SerializeObject(
                                reading, new JsonSerializerSettings() { ContractResolver = new CamelCasePropertyNamesContractResolver() })));

                    message.Properties.Add("couldRead", reading.CouldRead ? "true" : "false");

                    _logger.LogInformation($"Sending reading [{reading.Celcius.ToString("F")}°C] to Azure IoT Hub.");
                    await _deviceClient.SendEventAsync(message, cancellationToken);
                }
                catch (Exception exception)
                {
                    _logger.LogError(exception, "Unable to send CPU temperature to Azure IoT Hub.");
                }

                await Task.Delay(readInterval); //TOOD: Use a Timer instead.
            }
        }
    }
}