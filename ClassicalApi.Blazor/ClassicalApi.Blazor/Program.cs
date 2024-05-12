using ClassicalApi.Blazor.Components;
using ClassicalApi.Blazor.Components.Account;
using ClassicalApi.Blazor.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Radzen;
using ClassicalApi.Blazor.Client.Services;
using ClassicalApi.Blazor.Services;
using Microsoft.AspNetCore.HttpOverrides;
using ClassicalApi.Blazor;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("private-data.json");

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();
builder.Services.AddRadzenComponents();


builder.Services.AddScoped<IComposerService, ComposerService>();
builder.Services.AddHttpClient("ApiServer").ConfigureHttpClient(opt =>
{
    opt.BaseAddress = new Uri(builder.Configuration["ApiService:Url"] ?? "");
    opt.DefaultRequestHeaders.Add("x-api-key", builder.Configuration["ApiService:ApiKey"]);
});

builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

builder.Services.ConfigureAuthentication(builder.Configuration);
builder.Services.ConfigureAuthorization();


var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentityCore<AppUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();

builder.Services.AddSingleton<IEmailSender<AppUser>, IdentityNoOpEmailSender>();
bool useHttpsRequestSceme = builder.Configuration.GetValue<bool>("UseHttpsRequestSchene");

var app = builder.Build();

app.UseForwardedHeaders(new ForwardedHeadersOptions()
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

if (useHttpsRequestSceme)
{
    app.Use((context, next) =>
    {
        if (context.Request.Headers.TryGetValue("X-Forwarded-Proto", out var protoHeaderValue) &&
                protoHeaderValue == "https")
        {
            context.Request.Scheme = "https";
        }
        return next();
    });
}


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

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(ClassicalApi.Blazor.Client._Imports).Assembly);

app.MapAdditionalIdentityEndpoints();

//using (var scope = app.Services.CreateScope())
//{
//    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
//    await roleManager.CreateAsync(new("Administrator"));
//    await roleManager.CreateAsync(new("SuperAdmin"));
//    await roleManager.CreateAsync(new("User"));
//}

app.Run();