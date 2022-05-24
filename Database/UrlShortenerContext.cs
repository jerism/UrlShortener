using System;
using Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Database
{
    public class UrlShortenerContext : DbContext
    {
        public DbSet<Url> Urls { get; set; }

        public string DbPath { get; }

        public UrlShortenerContext(DbContextOptions<UrlShortenerContext> options) : base(options)
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = System.IO.Path.Join(path, "urlshortener.db");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlite($"Data Source={DbPath}");
        }
    }
}
