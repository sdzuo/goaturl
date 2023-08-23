using Microsoft.EntityFrameworkCore;
using urlgoatbackend.Models;

namespace urlgoatbackend.Data
{
    // DataContext is responsible for defining the database context
    public class DataContext : DbContext
    {
        // Constructor that accepts DbContextOptions
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        { }

        // DbSet represents the database table for UrlMapping entities
        public DbSet<UrlMapping> UrlMappings { get; set; }
    }
}
