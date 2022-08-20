using Moq;
using server.Processors;
using server.Services;
using Xunit;

namespace Server.Test
{
    public class TemperatureProcessorTest
    {
        private Mock<IHttpClientService> httpClientService = new Mock<IHttpClientService>();

        [Fact]
        public void TemperatureProcessor_None_Numeric_Id() 
        {
            httpClientService.Setup(x => x.GetAsync("")).ReturnsAsync((string)null);
            var temperatureProcessor = new TemperatureProcessor(httpClientService.Object);
            var response = temperatureProcessor.GetSensorData("a101");
            Assert.Null(response.Result);
        }

        [Fact]
        public void TemperatureProcessor_Negative_Id()
        {
            httpClientService.Setup(x => x.GetAsync("")).ReturnsAsync((string)null);
            var temperatureProcessor = new TemperatureProcessor(httpClientService.Object);
            var response = temperatureProcessor.GetSensorData("-5");
            Assert.Null(response.Result);
        }

        [Fact]
        public void TemperatureProcessor_Valid_Id()
        {
            var jsonString = "{\"id\":\"10\",\"temperature\":-2}";
            httpClientService.Setup(x => x.GetAsync("10")).ReturnsAsync(jsonString);
            var temperatureProcessor = new TemperatureProcessor(httpClientService.Object);
            var response = temperatureProcessor.GetSensorData("10");
            Assert.NotNull(response.Result);
        }
    }
}