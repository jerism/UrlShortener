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
        /// Retrieve a unique identifer by the original url
        /// </summary>
        /// <param name="originalUrl">Original Url to search by</param>
        /// <returns>Url object found</returns>
        Url GetByOriginalUrl(string originalUrl);

        /// <summary>
        /// Retrieve the original url with the unique idenifier
        /// </summary>
        /// <param name="uniqueIdenifier">Unique Idenifier to search by</param>
        /// <returns>Url object found</returns>
        Url GetByUniqueIdentifier(string uniqueIdenifier);

        /// <summary>
        /// Check whether a url already exists in the repository
        /// </summary>
        /// <param name="originalUrl">Original Url to check</param>
        /// <returns>True if url exists</returns>
        bool Exists(string originalUrl);

        /// <summary>
        /// Check whether a unique identifer already exists in the repository
        /// </summary>
        /// <param name="uniqueIdentifer">Unique Identifer to check</param>
        /// <returns>True if id exists</returns>
        bool UniqueIdentiferExists(string uniqueIdentifer);

        /// <summary>
        /// Delete a url from the repository
        /// </summary>
        /// <param name="id">Primary key of url to remove</param>
        /// <returns>True if successful</returns>
        Task<bool> DeleteAsync(int id);
    }
}
