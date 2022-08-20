using server.Models;
using server.Services;
using System.Text.Json;
using System.Threading.Tasks;

namespace server.Processors
{
    public class TemperatureProcessor : ITemperatureProcessor
    {
        private readonly IHttpClientService httpClientService;
        public TemperatureProcessor(IHttpClientService httpClientService)
        {
            this.httpClientService = httpClientService;
        }

        public async Task<Sensor> GetSensorData(string id)
        {
            if (!IsValidId(id))
            {
                return null;
            }

            var jsonString = await httpClientService.GetAsync(id);
            var sensorData = JsonSerializer.Deserialize<Sensor>(jsonString, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            return sensorData;
        }

        private static bool IsValidId(string id)
        {
            var isValidId = int.TryParse(id, out int parsedId);

            if (isValidId && parsedId < 0)
            {
                isValidId = false;
            }

            return isValidId;
        }
    }
}
