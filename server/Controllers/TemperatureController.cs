using Microsoft.AspNetCore.Mvc;
using server.Processors;
using System.Threading.Tasks;

namespace server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TemperatureController : ControllerBase
    {
        private readonly ITemperatureProcessor temperatureProcessor;

        public TemperatureController(ITemperatureProcessor temperatureProcessor)
        {
            this.temperatureProcessor = temperatureProcessor;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var sensorData = await temperatureProcessor.GetSensorData(id);

            if (sensorData == null)
            {
                return BadRequest("Provided id is not valid");
            }
            return Ok(sensorData);
        }
    }
}
