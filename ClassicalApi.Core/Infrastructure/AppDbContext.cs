using ClassicalApi.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace ClassicalApi.Core.Infrastructure;

public class AppDbContext : DbContext
{
    public DbSet<Composer> Composers { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}
