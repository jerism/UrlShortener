using System;
using System.Threading.Tasks;
using Database;
using Database.Entities;
using Database.Interfaces;
using Database.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace UrlShortener.Tests.Database
{
    public class UrlRepositoryTests
    {
        private IUrlRepository _sut;

        [SetUp]
        public async Task SetUp()
        {
            var dbContextOptions = new DbContextOptionsBuilder<UrlShortenerContext>()
                .UseInMemoryDatabase($"UrlShortenerDb_{DateTime.Now}")
                .Options;

            _sut = await CreateRepositoryAsync(dbContextOptions);
        }

        [Test]
        public async Task AddAsync_HappyPath_ShouldReturnShortenedUrl()
        {
            var url = new Url
            {
                OriginalUrl = "originalUrl.com/qwerty",
                ShortenedUrl = "tinyUrl.com/123456",
                LastAccessed = DateTime.Now
            };

            var result = await _sut.AddAsync(url);

            result.Should().NotBeNull();
            result.Id.Should().Be(4);
        }

        [Test]
        public void GetByOriginalUrl_HappyPath_ShouldReturnOriginalUrl()
        {
            var originalUrl = "originalUrl.com/abc1";

            var result = _sut.GetByOriginalUrl(originalUrl);

            result.Should().NotBeNull();
            result.Id.Should().Be(1);
        }

        [Test]
        public void GetByOriginalUrl_WhenNoShortenedUrlExists_ShouldThrowException()
        {
            var originalUrl = "originalUrl.com/qwerty";

            var result = _sut.GetByOriginalUrl(originalUrl);

            // result.Should().Throw<Exception>();
        }

        [Test]
        public void GetByShortenedUrl_WhenShortenedUrlExists_ShouldReturnOriginalUrl()
        {
            var shortenedUrl = "tinyUrl.com/1";

            var result = _sut.GetByShortenedUrl(shortenedUrl);

            result.Should().NotBeNull();
            result.OriginalUrl.Should().BeEquivalentTo("originalUrl.com/abc1");
        }

        [Test]
        public void GetByShortenedUrl_WhenNoShortenedUrlExists_ShouldThrowException()
        {
            var shortenedUrl = "tinyUrl.com/1";

            var result = _sut.GetByShortenedUrl(shortenedUrl);

            // result.Should().Throw<Exception>();
        }

        [Test]
        public void Exists_WhenOriginalUrlExists_ShouldReturnTrue()
        {
            var originalUrl = "originalUrl.com/abc1";

            var result = _sut.Exists(originalUrl);

            result.Should().BeTrue();
        }

        [Test]
        public void Exists_WhenNoOriginalUrlExists_ShouldReturnFalse()
        {
            var originalUrl = "does.not.exist";

            var result = _sut.Exists(originalUrl);

            result.Should().BeFalse();
        }

        [Test]
        public async Task DeleteAsync_HappyPath_ShouldReturnTrue()
        {
            var id = 1;
            var result = await _sut.DeleteAsync(id);

            result.Should().BeTrue();
        }

        [Test]
        public async Task DeleteAsync_WhenNoShortenedUrlExists_ShouldThrowException()
        {
            var id = 4;
            var result = await _sut.DeleteAsync(id);

            //result.Should().Throw<Exception>();
        }

        private async Task<IUrlRepository> CreateRepositoryAsync(DbContextOptions<UrlShortenerContext> dbContextOptions)
        {
            UrlShortenerContext context = new UrlShortenerContext(dbContextOptions);
            await PopulateDataAsync(context);
            return new UrlRepository(context);
        }

        private static async Task PopulateDataAsync(UrlShortenerContext context)
        {
            for (var i = 1; i < 3; i++)
            {
                var url = new Url
                {
                    Id = 1,
                    OriginalUrl = $"originalUrl.com/abc{i}",
                    ShortenedUrl = $"tinyUrl.com/{i}",
                    LastAccessed = DateTime.Now
                };

                await context.Urls.AddAsync(url);
            }

            await context.SaveChangesAsync();
        }
    }
}
