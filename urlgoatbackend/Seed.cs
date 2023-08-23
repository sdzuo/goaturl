using urlgoatbackend.Data;
using urlgoatbackend.Models;

namespace urlgoatbackend
{
    public class Seed
    {
        private readonly DataContext dbContext;

        public Seed(DataContext context)
        {
            dbContext = context;
        }

        public void SeedData()
        {
            // Check if there are no existing URL mappings in the database.
            if (!dbContext.UrlMappings.Any())
            {
                // Define a list of URL mappings to be seeded.
                var urlMappings = new List<UrlMapping>
                {
                    new UrlMapping
                    {
                        LongUrl = "https://www.example.com",
                        ShortKey = "abc123",
                    },
                    new UrlMapping
                    {
                        LongUrl = "https://www.anotherexample.com",
                        ShortKey = "xyz456",
                    }
                    // Add more URL mappings as needed
                };

                // Add the URL mappings to the database and save changes.
                dbContext.UrlMappings.AddRange(urlMappings);
                dbContext.SaveChanges();
            }
        }
    }
}
