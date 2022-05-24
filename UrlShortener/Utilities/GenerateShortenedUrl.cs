using System;
using Database.Interfaces;
using Microsoft.Extensions.Configuration;

namespace UrlShortener.Utilities
{
    public class GenerateShortenedUrl
    {
        private readonly string _characters;
        private readonly int _charactersLength;
        private readonly Random _random = new();
        private readonly IUrlRepository _urlRepository;

        public GenerateShortenedUrl(IConfiguration config, IUrlRepository urlRepository)
        {
            _urlRepository = urlRepository ?? throw new ArgumentNullException(nameof(urlRepository));
            _characters = config.GetValue<string>("BaseCharacters") ?? throw new NullReferenceException(nameof(_characters));
            _charactersLength = _characters.Length;
        }

        public string CreateUrl()
        {
            var uniqueIdentifer = CreateUniqueIdentifier();
            return $"{uniqueIdentifer}";
        }

        private string CreateUniqueIdentifier()
        {
            var uniqueId = string.Empty;

            for (var i = 0; i < 6; i++)
            {
                var index = _random.Next(_charactersLength);
                var character = _characters[index];
                uniqueId += character;
            }

            if (_urlRepository.UniqueIdentiferExists(uniqueId))
            {
                uniqueId = CreateUniqueIdentifier();
            }

            return uniqueId;
        }
    }
}
