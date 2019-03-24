using System.Threading;
using System.Threading.Tasks;

namespace RaspberryPiSensorNode.Temperature
{
    public class CpuTemperatureMonitor : ICpuTemperatureMonitor
    {
        public async Task Run(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                System.Console.WriteLine("test");
                await Task.Delay(1000);
            }
        }
    }
}