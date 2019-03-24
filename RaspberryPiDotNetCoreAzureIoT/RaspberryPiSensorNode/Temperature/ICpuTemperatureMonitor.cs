using System.Threading;
using System.Threading.Tasks;

namespace RaspberryPiSensorNode.Temperature
{
    public interface ICpuTemperatureMonitor
    {
        Task Run(CancellationToken cancellationToken);
    }
}