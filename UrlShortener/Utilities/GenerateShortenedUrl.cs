using System;
using Microsoft.Extensions.Configuration;

namespace UrlShortener.Utilities
{
    public class GenerateShortenedUrl
    {
        private readonly string _characters;
        private readonly int _charactersLength;
        private readonly Random _random = new();

        public GenerateShortenedUrl(IConfiguration config)
        {
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

            // TODO validate identifier is unique

            for (var i = 0; i < 6; i++)
            {
                var index = _random.Next(_charactersLength);
                var character = _characters[index];
                uniqueId += character;
            }

            return uniqueId;
        }
    }
}
