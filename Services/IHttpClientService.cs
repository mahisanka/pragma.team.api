using System.Threading.Tasks;

namespace server.Services
{
    public interface IHttpClientService
    {
        Task<string> GetAsync(string id);
    }
}