using Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Database
{
    public class UrlShortenerContext : DbContext
    {
        public DbSet<Url> Urls { get; set; }

        public UrlShortenerContext(DbContextOptions<UrlShortenerContext> options) : base(options)
        {

        }
    }
}
