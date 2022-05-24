using System.Threading.Tasks;
using Database.Entities;

namespace Database.Interfaces
{
    public interface IUrlRepository
    {
        Task<Url> AddAsync(Url url);
        Url GetByOriginalUrl(string originalUrl);
        Url GetByShortenedUrl(string shortenedUrl);
        bool Exists(string originalUrl);
        Task<bool> DeleteAsync(int id);
    }
}
