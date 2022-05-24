using System.Threading.Tasks;

namespace UrlShortener.Services
{
    public interface IUrlService
    {
        bool ValidateUrl(string url);
        bool CheckUrlExists(string url);
        Task<string> CreateUrlAsync(string url);
        string GetUrl(string url);
    }
}
