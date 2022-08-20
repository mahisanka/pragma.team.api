using Microsoft.Extensions.Configuration;
using Moq;
using Moq.Protected;
using server.Services;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Server.Test
{
    public class HttpClientServiceTest
    {
        private Mock<IHttpClientFactory> mockClientFactory = new Mock<IHttpClientFactory>();
        private Mock<HttpMessageHandler> mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        private Mock<IConfiguration> mockConfiguration = new Mock<IConfiguration>();

        private void SetUpConfiguration()
        {
            mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Services:SensorURL")]).Returns("https://temperature-sensor-service.herokuapp.com/sensor");
        }

        [Fact]
        public async Task HttpClientService_Success_Request()
        {
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("{\"id\":\"10\",\"temperature\":-2}")
                });

            var client = new HttpClient(mockHttpMessageHandler.Object);
            mockClientFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(client);

            SetUpConfiguration();

            HttpClientService httpClientService = new HttpClientService(mockClientFactory.Object, mockConfiguration.Object);
            var response = await httpClientService.GetAsync("10");
            Assert.NotNull(response);
        }

        [Fact]
        public async Task HttpClientService_Bad_Request()
        {
            var mockClientFactory = new Mock<IHttpClientFactory>();
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.BadRequest
                });

            var client = new HttpClient(mockHttpMessageHandler.Object);
            mockClientFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(client);

            SetUpConfiguration();

            HttpClientService httpClientService = new HttpClientService(mockClientFactory.Object, mockConfiguration.Object);
            var response = await httpClientService.GetAsync("10");
            Assert.Null(response);
        }
    }
}
