using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace server.Services
{
    public class HttpClientService : IHttpClientService
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly IConfiguration configuration;
        public HttpClientService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            this.httpClientFactory = httpClientFactory;
            this.configuration = configuration;
        }

        public async Task<string> GetAsync(string id)
        {
            var uri = string.Format("{0}/{1}", configuration["Services:SensorURL"], id);
            var client = httpClientFactory.CreateClient();
            var response = await client.GetAsync(uri);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                return null;
            }

            return await response.Content.ReadAsStringAsync();
        }
    }
}