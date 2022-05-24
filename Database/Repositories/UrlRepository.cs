using System;
using System.Linq;
using System.Threading.Tasks;
using Database.Entities;
using Database.Interfaces;

namespace Database.Repositories
{
    public class UrlRepository : IUrlRepository
    {
        private readonly UrlShortenerContext Context;

        public UrlRepository(UrlShortenerContext context)
        {
            Context = context;
        }

        public async Task<Url> AddAsync(Url url)
        {
            Context.Urls.Add(url);
            await Context.SaveChangesAsync();
            return url;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var url = await Context.Urls.FindAsync(id);

            if (url == null)
            {
                throw new Exception($"No url could be found with id {id}");
            }

            Context.Urls.Remove(url);
            await Context.SaveChangesAsync();

            return true;
        }

        public bool Exists(string originalUrl)
        {
            return Context.Urls.Any(u => u.OriginalUrl == originalUrl);
        }

        public Url GetByOriginalUrl(string originalUrl)
        {
            var url = Context.Urls.SingleOrDefault(u => u.OriginalUrl == originalUrl);

            if (url == null)
            {
                throw new Exception($"No url could be found with original url {originalUrl}");
            }

            return url;
        }

        public Url GetByShortenedUrl(string shortenedUrl)
        {
            var url = Context.Urls.SingleOrDefault(u => u.ShortenedUrl == shortenedUrl);

            if (url == null)
            {
                throw new Exception($"No url could be found with shortened url {shortenedUrl}");
            }

            return url;
        }
    }
}
