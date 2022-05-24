using System.Collections.Generic;
using Database.Interfaces;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using UrlShortener.Utilities;

namespace UrlShortener.Tests.Web.Utilities
{
    [TestFixture]
    public class GenerateShortenedUrlTests
    {
        private GenerateShortenedUrl _sut;
        private Mock<IUrlRepository> _mockUrlRepository;

        [SetUp]
        public void SetUp()
        {
            var inMemorySettings = new Dictionary<string, string> {
                { "BaseCharacters", "abcdefghijklmnopqrstuvwxyz0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ" }
            };

            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            _mockUrlRepository = new Mock<IUrlRepository>();
            _sut = new GenerateShortenedUrl(configuration, _mockUrlRepository.Object);
        }

        [Test]
        public void CreateUrl_WithBasUrl_ShouldCreateNewLink()
        {
            var result = _sut.CreateUrl();

            result.Should().MatchRegex(@"\w{6}");
            result.Should().HaveLength(6);
        }
    }
}
