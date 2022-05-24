using System.Collections.Generic;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using UrlShortener.Utilities;

namespace UrlShortener.Tests.Web.Utilities
{
    [TestFixture]
    public class GenerateShortenedUrlTests
    {
        private GenerateShortenedUrl _sut;

        [SetUp]
        public void SetUp()
        {
            var inMemorySettings = new Dictionary<string, string> {
                { "BaseCharacters", "abcdefghijklmnopqrstuvwxyz0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ" }
            };

            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            _sut = new GenerateShortenedUrl(configuration);
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
