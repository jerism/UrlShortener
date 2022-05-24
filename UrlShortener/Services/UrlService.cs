using System;
using System.Threading.Tasks;
using Database.Entities;
using Database.Interfaces;
using Microsoft.Extensions.Configuration;
using UrlShortener.Utilities;

namespace UrlShortener.Services
{
    public class UrlService : IUrlService
    {
        private readonly IUrlRepository _urlRepository;
        private readonly GenerateShortenedUrl _generateShortenedUrl;
        private readonly string _baseUrl;

        public UrlService(IUrlRepository urlRepository, GenerateShortenedUrl generateShortenedUrl, IConfiguration config)
        {
            _urlRepository = urlRepository ?? throw new ArgumentNullException(nameof(urlRepository));
            _generateShortenedUrl = generateShortenedUrl ?? throw new ArgumentNullException(nameof(generateShortenedUrl));
            _baseUrl = config.GetValue<string>("BaseUrl") ?? throw new NullReferenceException(nameof(_baseUrl));
        }

        public bool CheckUrlExists(string url)
        {
            return _urlRepository.Exists(url);
        }

        public async Task<string> CreateUrlAsync(string url)
        {
            if (CheckUrlExists(url))
            {
                return _urlRepository.GetByOriginalUrl(url).ShortenedUrl;
            }

            var shortenedUrl = _generateShortenedUrl.CreateUrl();

            var urlToAdd = new Url
            {
                OriginalUrl = url,
                ShortenedUrl = shortenedUrl,
                LastAccessed = DateTime.Now
            };

            var createdUrl = await _urlRepository.AddAsync(urlToAdd);
            return $@"{_baseUrl}\{createdUrl.ShortenedUrl}";
        }

        public string GetUrl(string url)
        {
            var originalUrl = string.Empty;

            // TODO implement validation
            if (url.StartsWith("www.test.com")) { }

            try
            {
                originalUrl = _urlRepository.GetByShortenedUrl(url).OriginalUrl;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return originalUrl;
        }

        public bool ValidateUrl(string url)
        {
            throw new System.NotImplementedException();
        }
    }
}
