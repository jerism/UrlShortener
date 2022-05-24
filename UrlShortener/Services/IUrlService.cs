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
        /// Check if the url already has a unqiue identifier
        /// </summary>
        /// <param name="url">Original Url</param>
        bool CheckUrlExists(string url);

        /// <summary>
        /// Create a new unqiue identifier and save
        /// </summary>
        /// <param name="url">Original url</param>
        /// <returns>Unqiue identifier</returns>
        Task<string> CreateUrlAsync(string url);

        /// <summary>
        /// Retrieve the original url for a given unqiue identifier
        /// </summary>
        /// <param name="uid">Unique Identifier</param>
        /// <returns>Original Url</returns>
        string GetUrl(string uid);
    }
}
