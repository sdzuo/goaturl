using Microsoft.EntityFrameworkCore;
using urlgoatbackend.Data;
using urlgoatbackend.Interfaces;
using urlgoatbackend.Models;

namespace urlgoatbackend.Repository
{
    public class UrlMappingRepository : IUrlMappingRepository
    {
        private readonly DataContext _context;

        public UrlMappingRepository(DataContext context)
        {
            _context = context;
        }

        // Create a short URL mapping and add it to the database
        public void CreateShortUrl(UrlMapping urlMapping)
        {
            _context.Add(urlMapping);
        }

        // Retrieve the original URL mapping by short key asynchronously
        public async Task<UrlMapping> GetOriginalUrlByShortKeyAsync(string shortKey)
        {
            try
            {
                return await _context.UrlMappings.FirstOrDefaultAsync(u => u.ShortKey == shortKey);
            }
            catch (Exception ex)
            {
                // Log the exception for debugging purposes
                Console.WriteLine($"Error in GetOriginalUrlByShortKeyAsync: {ex.Message}");
                throw; // Rethrow the exception to propagate it to the caller
            }
        }


        // Retrieve a URL mapping by long URL asynchronously
        public async Task<UrlMapping> GetUrlMappingByUrl(string longUrl)
        {
            return await _context.UrlMappings.FirstOrDefaultAsync(u => u.LongUrl == longUrl);
        }

        // Save changes to the database asynchronously
        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        // Check if a short key exists in the database
        public bool ShortKeyExists(string shortKey)
        {
            return _context.UrlMappings.Any(u => u.ShortKey == shortKey);
        }

        // Check if a URL with the given ID exists in the database
        public bool UrlExists(int urlId)
        {
            return _context.UrlMappings.Any(u => u.Id == urlId);
        }
    }
}
