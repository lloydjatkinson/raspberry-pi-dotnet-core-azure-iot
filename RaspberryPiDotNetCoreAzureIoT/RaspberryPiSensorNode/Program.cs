using System;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using RaspberryPiSensorNode.Configuration;
using RaspberryPiSensorNode.Temperature;

namespace RaspberryPiSensorNode
{
    internal class Program
    {
        private readonly static CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        public static async Task Main(string[] args)
        {
            var serviceProvider = BuildCompositionRoot(new ServiceCollection());

            // Entry Point
            Console.CancelKeyPress += HandleExit;
            await serviceProvider.GetService<App>().Run(_cancellationTokenSource.Token);

            // This isn't needed per se however console logging is done on a queue in another thread for performance.
            // That queue might not be flushed by the time the console application exits.
            (serviceProvider as IDisposable)?.Dispose();
        }

        private static ServiceProvider BuildCompositionRoot(IServiceCollection servicesCollection)
        {
            // Configuration
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            var serviceProvider = new ServiceCollection()
                .AddOptions()
                .AddLogging(loggingBuilder => loggingBuilder.AddConsole())
                .Configure<AzureIoTHubConfiguration>(configuration.GetSection("AzureIoTHub"))
                .AddTransient<ITemperatureReader, RaspberryPiCpuTemperatureReader>()
                .AddTransient<ICpuTemperatureMonitor, CpuTemperatureMonitor>()
                .AddTransient<App>()
                .BuildServiceProvider();

            return serviceProvider;
        }

        private static void HandleExit(object sender, ConsoleCancelEventArgs eventArguments)
        {
            eventArguments.Cancel = true;
            _cancellationTokenSource.Cancel();
        }
    }
}