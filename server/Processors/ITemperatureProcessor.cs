using server.Models;
using System.Threading.Tasks;

namespace server.Processors
{
    public interface ITemperatureProcessor
    {
        public Task<Sensor> GetSensorData(string id);
    }
}
