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

        /// <summary>
        /// Check if the url already has a shortened url
        /// </summary>
        /// <param name="url">Original Url</param>
        public bool CheckUrlExists(string url)
        {
            return _urlRepository.Exists(url);
        }

        /// <summary>
        /// Create a new shortened url and save
        /// </summary>
        /// <param name="url">Original url</param>
        /// <returns>Shortened Url</returns>
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

        /// <summary>
        /// Retrieve the original url for a given short url
        /// </summary>
        /// <param name="url">Shortened url</param>
        /// <returns></returns>
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

        /// <summary>
        /// Validate the original url to ensure its a valid url
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public bool ValidateUrl(string url)
        {
            throw new System.NotImplementedException();
        }
    }
}
