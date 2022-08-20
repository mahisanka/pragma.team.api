using Microsoft.AspNetCore.Mvc;
using Moq;
using server.Controllers;
using server.Models;
using server.Processors;
using Xunit;

namespace Server.Test
{
    public class TemperatureControllerTest
    {
        private Mock<ITemperatureProcessor> temperatureProcessor = new Mock<ITemperatureProcessor>();

        [Fact]
        public void TemperatureController_BadRequest()
        {
            temperatureProcessor.Setup(x => x.GetSensorData("")).ReturnsAsync((Sensor)null);
            var controller = new TemperatureController(temperatureProcessor.Object);
            var result = controller.Get("a2").Result;
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void TemperatureController_Success()
        {
            var sensor = new Sensor { Id = "1", Temperature = 4 };
            temperatureProcessor.Setup(x => x.GetSensorData("10")).ReturnsAsync(sensor);
            var controller = new TemperatureController(temperatureProcessor.Object);
            var result = controller.Get("10").Result;
            var badRequestResult = Assert.IsType<OkObjectResult>(result);
        }
    }
}
