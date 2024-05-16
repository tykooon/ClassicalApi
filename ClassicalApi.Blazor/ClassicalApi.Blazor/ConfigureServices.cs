using ClassicalApi.Blazor.Authentication;
using ClassicalApi.Blazor.Components.Account;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;

namespace ClassicalApi.Blazor;

public static class ConfigureServices
{
    public static IServiceCollection ConfigureIdentityCore(this IServiceCollection services)
    {
        services.AddIdentityCore<AppUser>(options => options.SignIn.RequireConfirmedAccount = false)
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddSignInManager()
            .AddDefaultTokenProviders();
        services.AddScoped<IdentityUserAccessor>();
        services.AddScoped<IdentityRedirectManager>();
        services.AddSingleton<IEmailSender<AppUser>, IdentityNoOpEmailSender>();
        return services;
    }

    public static IServiceCollection ConfigureAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCascadingAuthenticationState();
        services.AddScoped<AuthenticationStateProvider, PersistingRevalidatingAuthenticationStateProvider>();

        bool forceSecureCookie = configuration.GetValue<bool>("ForceSecureCookie");

        var externalConfig = configuration.GetSection("ExternalLogins");
        services.AddAuthentication(options =>
        {
            options.DefaultScheme = IdentityConstants.ApplicationScheme;
            options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
        })
            .AddGoogle(googleOptions =>
            {
                googleOptions.ClientId = externalConfig["Google:ClientId"] ?? "";
                googleOptions.ClientSecret = externalConfig["Google:ClientSecret"] ?? "";
            }
            )
            .AddMicrosoftAccount(msOptions =>
            {
                msOptions.ClientId = externalConfig["Microsoft:ClientId"] ?? "";
                msOptions.ClientSecret = externalConfig["Microsoft:ClientSecret"] ?? "";
            })
            //.AddGitHub(githubOption =>
            //{
            //    githubOption.ClientId = externalConfig["GitHub:ClientId"] ?? "";
            //    githubOption.ClientSecret = externalConfig["GitHub:ClientSecret"] ?? "";
            //})
            .AddFacebook(fbOptions =>
            {
                fbOptions.AppId = externalConfig["Facebook:ClientId"] ?? "";
                fbOptions.AppSecret = externalConfig["Facebook:ClientSecret"] ?? ""; ;
                //fbOptions.Scope.Add("public_profile");
            })
            .AddIdentityCookies();

        return services;
    }

    public static IServiceCollection ConfigureAuthorization(this IServiceCollection services)
    {
        services.AddAuthorization(opt =>
        {
            opt.AddPolicy("Regular", policy => policy.RequireAuthenticatedUser());
            opt.AddPolicy("Admin", policy =>
            {
                policy.RequireRole("Administrator", "SuperAdmin");
            });
            opt.AddPolicy("FullAccess", policy =>
            {
                policy.RequireRole("SuperAdmin");
            });
        });

        return services;
    }
}
