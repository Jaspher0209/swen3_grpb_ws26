using Microsoft.EntityFrameworkCore;
using Models;

namespace Dal;

public class MetadataDbContext : DbContext
{
    public DbSet<MetaData> MetaData { get; set; }
    
    // Constructor
    public MetadataDbContext(DbContextOptions<MetadataDbContext> options) : base(options) {}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
    }
}