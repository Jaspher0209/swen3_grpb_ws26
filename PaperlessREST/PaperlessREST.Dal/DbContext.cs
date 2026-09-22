using Microsoft.EntityFrameworkCore;
using PaperlessREST.Models;

namespace PaperlessREST.Dal;

public class DbContext : Microsoft.EntityFrameworkCore.DbContext
{
    public DbSet<MetaData> MetaData { get; set; }
    public DbSet<ShareLink> ShareLink { get; set; }
    // Constructor
    public DbContext(DbContextOptions<DbContext> options) : base(options) {}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
    }
}