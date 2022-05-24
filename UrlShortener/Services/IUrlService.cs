using System.Threading.Tasks;

namespace UrlShortener.Services
{
    public interface IUrlService
    {

        /// <summary>
        /// Validate the original url to ensure its a valid url
        /// </summary>
        /// <param name="url"></param>
        /// <returns>True if valid url</returns>
        bool ValidateUrl(string url);

        /// <summary>
        /// Check if the url already has a shortened url
        /// </summary>
        /// <param name="url">Original Url</param>
        bool CheckUrlExists(string url);

        /// <summary>
        /// Create a new shortened url and save
        /// </summary>
        /// <param name="url">Original url</param>
        /// <returns>Shortened Url</returns>
        Task<string> CreateUrlAsync(string url);

        /// <summary>
        /// Retrieve the original url for a given short url
        /// </summary>
        /// <param name="url">Shortened url</param>
        /// <returns>Original Url</returns>
        string GetUrl(string url);
    }
}
