using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RaspberryPiSensorDevice.Configuration;

namespace RaspberryPiSensorDevice
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var serviceProvider = BuildCompositionRoot(new ServiceCollection());

            // Entry Point
            await serviceProvider.GetService<App>().Run();

            // This isn't needed per se however console logging is done on a queue in another thread for performance.
            // That queue might not be flushed by the time the console application exits.
            (serviceProvider as IDisposable)?.Dispose();
        }

        private static ServiceProvider BuildCompositionRoot(IServiceCollection servicesCollection)
        {
            // Configuration
            var configuration = new ConfigurationBuilder()
                //.SetBasePath(AppContext.BaseDirectory)
                //.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            var serviceProvider = new ServiceCollection()
                .AddOptions()
                .AddLogging(loggingBuilder => loggingBuilder.AddConsole())
                .Configure<AzureIoTHubConfiguration>(configuration.GetSection("AzureIoTHub"))
                .AddTransient<ICpuTemperatureReader, RaspberryPiCpuTemperatureReader>()
                .AddTransient<App>()
                .BuildServiceProvider();

            return serviceProvider;
        }
    }
}