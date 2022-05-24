using System.Threading.Tasks;
using Database.Entities;

namespace Database.Interfaces
{
    public interface IUrlRepository
    {
        /// <summary>
        /// Add a new url to the repository
        /// </summary>
        /// <param name="url">Url to be added</param>
        /// <returns>Created Url</returns>
        Task<Url> AddAsync(Url url);

        /// <summary>
        /// Retrieve a shortened url by the original url
        /// </summary>
        /// <param name="originalUrl">Original Url to search by</param>
        /// <returns>Url object found</returns>
        Url GetByOriginalUrl(string originalUrl);

        /// <summary>
        /// Retrieve the original url with the shortened url
        /// </summary>
        /// <param name="shortenedUrl">Shortened Url to search by</param>
        /// <returns>Url object found</returns>
        Url GetByShortenedUrl(string shortenedUrl);

        /// <summary>
        /// Check whether a url already exists in the repository
        /// </summary>
        /// <param name="originalUrl">Original Url to check</param>
        /// <returns>True if url exists</returns>
        bool Exists(string originalUrl);

        /// <summary>
        /// Delete a url from the repository
        /// </summary>
        /// <param name="id">Primary key of url to remove</param>
        /// <returns>True if successful</returns>
        Task<bool> DeleteAsync(int id);
    }
}
