using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace ClassicalApi.Blazor.Authentication;

public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<AppUser>(options)
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<AppUser>()
            .Property(user => user.FavoriteMediaIds)
            .HasConversion<FavotiteMediaIdsConverter,FavoriteMediaIdsComparer>();
    }
}
