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
            string uid;

            if (CheckUrlExists(url))
            {
                uid = _urlRepository.GetByOriginalUrl(url).UniqueIdentifier;
                return $"{_baseUrl}/{uid}";
            }

            uid = _generateShortenedUrl.CreateUrl();

            var urlToAdd = new Url
            {
                OriginalUrl = url,
                UniqueIdentifier = uid
            };

            var createdUrl = await _urlRepository.AddAsync(urlToAdd);
            return $"{_baseUrl}/{createdUrl.UniqueIdentifier}";
        }

        public string GetUrl(string uid)
        {
            var originalUrl = string.Empty;

            try
            {
                originalUrl = _urlRepository.GetByUniqueIdentifier(uid).OriginalUrl;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return originalUrl;
        }

        public bool ValidateUrl(string url)
        {
            if (!url.StartsWith("http://") && !url.StartsWith("https://"))
            {
                return false;
            }

            return true;
        }
    }
}
