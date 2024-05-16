using Microsoft.AspNetCore.Identity;

namespace ClassicalApi.Blazor.Helpers;

public static class SeedExtensions
{
    public static async Task SeedIdentityRoles(this WebApplication app)
    {
        if (app == null) 
        {
            return;
        }

        using (var scope = app.Services.CreateScope())
        {
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            await roleManager.CreateAsync(new("Administrator"));
            await roleManager.CreateAsync(new("SuperAdmin"));
            await roleManager.CreateAsync(new("User"));
        }
    }
}
