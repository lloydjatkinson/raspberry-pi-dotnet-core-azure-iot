using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Azure.Devices.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

using RaspberryPiSensorNode.Configuration;

namespace RaspberryPiSensorNode.Temperature
{
    public class CpuTemperatureMonitor : ICpuTemperatureMonitor
    {
        private readonly ILogger<CpuTemperatureMonitor> _logger;
        private readonly IOptions<AzureIoTHubConfiguration> _azureIoTHubConnectionConfiguration; //TODO: Look into removing dependency on IOptions in here.
        private readonly ITemperatureReader _temperatureReader;
        private readonly DeviceClient _deviceClient;


        public CpuTemperatureMonitor(ILogger<CpuTemperatureMonitor> logger, IOptions<AzureIoTHubConfiguration> azureIoTHubConnectionConfiguration, ITemperatureReader temperatureReader)
        {
            _logger = logger;
            _azureIoTHubConnectionConfiguration = azureIoTHubConnectionConfiguration;
            _temperatureReader = temperatureReader;
            _deviceClient = DeviceClient.CreateFromConnectionString(_azureIoTHubConnectionConfiguration.Value.ConnectionString, TransportType.Mqtt);
        }

        public async Task Run(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    _logger.LogInformation("Reading temperature...");
                    var reading = _temperatureReader.Read();
                    //var reading = new TemperatureReading(true, 20d);
                    _logger.LogInformation($"Temperature: {reading.Celcius}");

                    var message = new Message(
                        Encoding.ASCII.GetBytes(
                            JsonConvert.SerializeObject(
                                reading, new JsonSerializerSettings() { ContractResolver = new CamelCasePropertyNamesContractResolver() })));

                    message.Properties.Add("couldRead", reading.CouldRead ? "true" : "false");

                    _logger.LogInformation("Sending reading to Azure IoT Hub.");
                    await _deviceClient.SendEventAsync(message, cancellationToken);
                }
                catch (Exception exception)
                {
                    _logger.LogError(exception, "Unable to send CPU temperature to Azure IoT Hub.");
                }

                await Task.Delay(TimeSpan.FromSeconds(10)); //TOOD: Use a Timer instead.
            }
        }
    }
}
