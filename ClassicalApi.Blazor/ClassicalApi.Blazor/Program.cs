using ClassicalApi.Blazor.Components;
using ClassicalApi.Blazor.Authentication;
using Microsoft.EntityFrameworkCore;
using Radzen;
using ClassicalApi.Blazor.Client.Services;
using ClassicalApi.Blazor.Services;
using Microsoft.AspNetCore.HttpOverrides;
using ClassicalApi.Blazor;
using ClassicalApi.Blazor.Middleware;
using ClassicalApi.Blazor.Helpers;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("private-data.json");
var appSettings = new AppSettings(builder.Configuration);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();
builder.Services.AddRadzenComponents();

builder.Services.AddScoped<IComposerService, ComposerService>();
builder.Services.AddHttpClient("ApiServer").ConfigureHttpClient(opt =>
{
    opt.BaseAddress = new Uri(appSettings.ApiServiceUrl);
    opt.DefaultRequestHeaders.Add("x-api-key", appSettings.ApiServiceKey);
});

builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

builder.Services.ConfigureAuthentication(builder.Configuration);
builder.Services.ConfigureAuthorization();
builder.Services.ConfigureIdentityCore();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

if (appSettings.ForceSecureCookie)
{
    builder.Services.AddDistributedMemoryCache();
    builder.Services.AddSession(options =>
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always);
}

var app = builder.Build();

app.UseLogHeaders(appSettings.LoggingHeaders);
app.UseHttpsScheme(appSettings.UseHttpsRequestScheme);
app.UseForwardedHeaders(new ForwardedHeadersOptions()
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto | ForwardedHeaders.XForwardedHost
});

if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
    //app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
}

app.UseResponseCaching();
app.UseStaticFiles();
app.UseAntiforgery();
//app.UseSession();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(ClassicalApi.Blazor.Client._Imports).Assembly);

app.MapAdditionalIdentityEndpoints();

//await app.SeedIdentityRoles();

app.Run();