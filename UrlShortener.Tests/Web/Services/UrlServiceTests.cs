using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Database.Entities;
using Database.Interfaces;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using UrlShortener.Services;
using UrlShortener.Utilities;

namespace UrlShortener.Tests.Web.Services
{
    [TestFixture]
    public class UrlServiceTests
    {
        private IUrlService _sut;
        private Mock<IUrlRepository> _mockUrlRepository;
        private GenerateShortenedUrl _mockGenerateShortenedUrl;

        [SetUp]
        public void SetUp()
        {
            var inMemorySettings = new Dictionary<string, string> {
                { "BaseUrl", "test.com" },
                { "BaseCharacters", "abcdefghijklmnopqrstuvwxyz0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ" }
            };

            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            _mockUrlRepository = new Mock<IUrlRepository>();
            var generateShortenedUrl = new GenerateShortenedUrl(configuration, _mockUrlRepository.Object);
            _sut = new UrlService(_mockUrlRepository.Object, generateShortenedUrl, configuration);
        }

        [Test]
        public void CheckUrlExists_WithExistingUrl_ShouldReturnTrue()
        {
            _mockUrlRepository.Setup(s => s.Exists("test.com")).Returns(true);

            var url = "test.com";

            var result = _sut.CheckUrlExists(url);

            result.Should().BeTrue();

            _mockUrlRepository.Verify(s => s.Exists(url), Times.Once);
        }

        [Test]
        public void CheckUrlExists_WithoutExistingUrl_ShouldReturnFalse()
        {
            _mockUrlRepository.Setup(s => s.Exists(It.IsAny<string>())).Returns(false);

            var url = "test.com";

            var result = _sut.CheckUrlExists(url);

            result.Should().BeFalse();

            _mockUrlRepository.Verify(s => s.Exists(url), Times.Once);
        }

        [Test]
        public void GetUrl_WithExistingUrl_ShouldReturnUrl()
        {
            var url = new Url
            {
                OriginalUrl = "test.com",
                UniqueIdentifier = "abcdef",
            };

            _mockUrlRepository.Setup(s => s.GetByUniqueIdentifier("abcdef")).Returns(url);

            var result = _sut.GetUrl("abcdef");

            result.Should().NotBeNull();
            result.Should().Be("test.com");
            _mockUrlRepository.Verify(s => s.GetByUniqueIdentifier("abcdef"), Times.Once);
        }

        [Test]
        public void GetUrl_WithoutExistingUrl_ShouldReturnStringEmpty()
        {
            _mockUrlRepository.Setup(s => s.GetByUniqueIdentifier(It.IsAny<string>())).Throws<Exception>();

            var result = _sut.GetUrl("test.com");

            result.Should().BeNullOrEmpty();

            _mockUrlRepository.Verify(s => s.GetByUniqueIdentifier("test.com"), Times.Once);
        }

        [Test]
        public async Task CreateUrlAsync_WithExistingUrl_ShouldReturnExistingUrl()
        {
            var url = new Url
            {
                OriginalUrl = "test.com",
                UniqueIdentifier = "abc",
            };

            _mockUrlRepository.Setup(s => s.Exists("test.com")).Returns(true);
            _mockUrlRepository.Setup(s => s.GetByOriginalUrl("test.com")).Returns(url);

            var result = await _sut.CreateUrlAsync("test.com");

            result.Should().NotBeNull();
            result.Should().Be("test.com/abc");
            _mockUrlRepository.Verify(s => s.GetByOriginalUrl("test.com"), Times.Once);
            _mockUrlRepository.Verify(s => s.AddAsync(It.IsAny<Url>()), Times.Never);
        }

        [Test]
        public async Task CreateUrlAsync_WithoutExistingUrl_ShouldReturnNewUrl()
        {
            var url = new Url
            {
                OriginalUrl = "test.com",
                UniqueIdentifier = "abc",
            };

            _mockUrlRepository.Setup(s => s.Exists("test.com")).Returns(false);

            _mockUrlRepository.Setup(s => s.AddAsync(It.IsAny<Url>())).ReturnsAsync(url);

            var result = await _sut.CreateUrlAsync("test.com");

            result.Should().NotBeNull();
            result.Should().Be("test.com/abc");

            _mockUrlRepository.Verify(s => s.GetByOriginalUrl("test.com"), Times.Never);
            _mockUrlRepository.Verify(s => s.AddAsync(It.IsAny<Url>()), Times.Once);
        }

        [Test]
        public void ValidateUrl_WithValidUrl_ShouldReturnTrue()
        {
            var result = _sut.ValidateUrl("https://test.com");
            result.Should().BeTrue();
        }

        [Test]
        public void ValidateUrl_WithValidUrl_ShouldReturnFalse()
        {
            var result = _sut.ValidateUrl("--invalid-test.com");
            result.Should().BeFalse();
        }
    }
}
