using ClassicalApi.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace ClassicalApi.Core.Infrastructure;

public class AppDbContext(DbContextOptions<AppDbContext> options) :
    DbContext(options)
{
    public DbSet<Composer> Composers { get; set; }
    public DbSet<Portrait> Portraits { get; set; }
    public DbSet<MediaLink> MediaLinks { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}
