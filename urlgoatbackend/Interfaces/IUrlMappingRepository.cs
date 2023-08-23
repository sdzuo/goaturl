using urlgoatbackend.Models;

namespace urlgoatbackend.Interfaces
{
    public interface IUrlMappingRepository
    {
        // Check if a URL with the given ID exists
        bool UrlExists(int urlId);

        // Get a URL mapping by its long URL
        Task<UrlMapping> GetUrlMappingByUrl(string url);

        // Check if a short key exists
        bool ShortKeyExists(string shortKey);

        // Create a new short URL mapping
        void CreateShortUrl(UrlMapping urlMapping);

        // Get the original URL by its short key asynchronously
        Task<UrlMapping> GetOriginalUrlByShortKeyAsync(string shortKey);

        // Save changes asynchronously
        Task SaveAsync();
    }
}
