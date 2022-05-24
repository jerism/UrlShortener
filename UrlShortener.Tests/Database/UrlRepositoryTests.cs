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
    [TestFixture]
    public class UrlRepositoryTests
    {
        private IUrlRepository _sut;
        private UrlShortenerContext _context;

        [OneTimeSetUp]
        public async Task OneTimeSetUp()
        {
            var dbContextOptions = new DbContextOptionsBuilder<UrlShortenerContext>()
                .UseInMemoryDatabase($"UrlShortenerDb_{DateTime.Now}")
                .Options;

            _context = new(dbContextOptions);
        }

        [SetUp]
        public async Task SetUp()
        {
            _sut = await CreateRepositoryAsync(_context);
        }

        [Test]
        public async Task AddAsync_HappyPath_ShouldReturnShortenedUrl()
        {
            var url = new Url
            {
                OriginalUrl = "originalUrl.com/qwerty",
                UniqueIdentifier = "123456"
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

            _sut.Invoking(_ => _.GetByOriginalUrl(originalUrl))
                    .Should().Throw<Exception>();
        }

        [Test]
        public void GetByShortenedUrl_WhenShortenedUrlExists_ShouldReturnOriginalUrl()
        {
            var shortenedUrl = "abcde1";

            var result = _sut.GetByUniqueIdentifier(shortenedUrl);

            result.Should().NotBeNull();
            result.OriginalUrl.Should().BeEquivalentTo("originalUrl.com/abc1");
        }

        [Test]
        public void GetByShortenedUrl_WhenNoShortenedUrlExists_ShouldThrowException()
        {
            var shortenedUrl = "404";

            _sut.Invoking(_ => _.GetByUniqueIdentifier(shortenedUrl))
                .Should().Throw<Exception>();
        }

        [Test]
        public void Exists_WhenOriginalUrlExists_ShouldReturnTrue()
        {
            var originalUrl = "originalUrl.com/abc1";

            var result = _sut.Exists(originalUrl);

            result.Should().BeTrue();
        }

        [Test]
        public void UniqueIdentiferExists_WhenNoUniqueIdExists_ShouldReturnFalse()
        {
            var uniqueId = "zzzzzz";

            var result = _sut.UniqueIdentiferExists(uniqueId);

            result.Should().BeFalse();
        }

        [Test]
        public void UniqueIdentiferExists_WhenUniqueIdExists_ShouldReturnTrue()
        {
            var uniqueId = "abcde1";

            var result = _sut.UniqueIdentiferExists(uniqueId);

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
            var id = 3;
            var result = await _sut.DeleteAsync(id);

            result.Should().BeTrue();
        }

        [Test]
        public void DeleteAsync_WhenNoShortenedUrlExists_ShouldThrowException()
        {
            var id = 404;

            _sut.Invoking(_ => _.DeleteAsync(id))
                    .Should().Throw<Exception>();
        }

        private static async Task<IUrlRepository> CreateRepositoryAsync(UrlShortenerContext context)
        {
            await SetUpData(context);
            return new UrlRepository(context);
        }

        private static async Task SetUpData(UrlShortenerContext context)
        {
            await context.Urls.ForEachAsync(url => context.Remove(url));

            for (var i = 1; i < 4; i++)
            {
                var url = new Url
                {
                    Id = i,
                    OriginalUrl = $"originalUrl.com/abc{i}",
                    UniqueIdentifier = $"abcde{i}"
                };

                await context.Urls.AddAsync(url);
            }

            await context.SaveChangesAsync();
        }
    }
}
