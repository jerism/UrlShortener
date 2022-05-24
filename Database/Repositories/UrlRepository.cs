using System;
using System.Linq;
using System.Threading.Tasks;
using Database.Entities;
using Database.Interfaces;

namespace Database.Repositories
{
    public class UrlRepository : IUrlRepository
    {
        private readonly UrlShortenerContext _context;

        public UrlRepository(UrlShortenerContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Url> AddAsync(Url url)
        {
            _context.Urls.Add(url);
            await _context.SaveChangesAsync();
            return url;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var url = await _context.Urls.FindAsync(id);

            if (url == null)
            {
                throw new Exception($"No url could be found with id {id}");
            }

            _context.Urls.Remove(url);
            await _context.SaveChangesAsync();

            return true;
        }

        public bool Exists(string originalUrl)
        {
            return _context.Urls.Any(u => u.OriginalUrl == originalUrl);
        }

        public bool UniqueIdentiferExists(string uniqueIdentifer)
        {
            return _context.Urls.Any(u => u.UniqueIdentifier == uniqueIdentifer);
        }

        public Url GetByOriginalUrl(string originalUrl)
        {
            var url = _context.Urls.SingleOrDefault(u => u.OriginalUrl == originalUrl);

            if (url == null)
            {
                throw new Exception($"No url could be found with original url {originalUrl}");
            }

            return url;
        }

        public Url GetByUniqueIdentifier(string uniqueIdenifier)
        {
            var url = _context.Urls.SingleOrDefault(u => u.UniqueIdentifier == uniqueIdenifier);

            if (url == null)
            {
                throw new Exception($"No url could be found with unqiue identifier: '{uniqueIdenifier}'");
            }

            return url;
        }
    }
}
