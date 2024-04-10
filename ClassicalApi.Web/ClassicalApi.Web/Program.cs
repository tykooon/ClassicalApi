using ClassicalApi.Web.Client.Services;
using ClassicalApi.Web.Components;
using ClassicalApi.Web.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using ClassicalApi.Web.Data;
using Radzen;
using ClassicalApi.Web.Components.Account;
using Microsoft.EntityFrameworkCore;
using ClassicalApi.Web.Client.Models;
using System.Security.Claims;
using Blazored.SessionStorage;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddRadzenComponents();

builder.Services.AddScoped<IComposerService, ComposerService>();
builder.Services.AddHttpClient();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, PersistingRevalidatingAuthenticationStateProvider>();

builder.Services.AddScoped<IAuthenticateService, AuthenticateService>();
builder.Services.AddBlazoredSessionStorage();
builder.Services.AddHttpClient("AuthServer").ConfigureHttpClient(opt =>
{
    opt.BaseAddress = new Uri("http://localhost:5100");
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = IdentityConstants.ApplicationScheme;
    options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
})
    .AddIdentityCookies();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false) // TODO: Email confirmation currently off
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();

builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
    app.UseSwagger();
    app.UseSwaggerUI();

}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
}

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(ClassicalApi.Web.Client._Imports).Assembly);

app.MapIdentityApi<ApplicationUser>();
//app.MapAdditionalIdentityEndpoints();

var account = app.MapGroup("account");

account.MapPost("/sign-in", async (
    SignInManager<ApplicationUser> signInManager,
    HttpContext httpContext,
    string returnUrl) =>
{
    var loginData = await httpContext.Request.ReadFromJsonAsync<LoginModel>() ?? new();
    var result = await signInManager.PasswordSignInAsync(loginData.Email, loginData.Password, loginData.RememberMe, lockoutOnFailure: false);
    return result;
});

account.MapGet("/profile", (
    UserManager<ApplicationUser> userManager,
    HttpContext httpContext) =>
{
    var user = httpContext.User;
    UserInfo result = new()
    {
        Email = user.Claims.FirstOrDefault(cl => cl.Type == ClaimTypes.Email)?.Value ?? "",
        UserId = user.Claims.FirstOrDefault(cl => cl.Type == ClaimTypes.NameIdentifier)?.Value ?? ""
    };
    return result;
});


app.Run();
