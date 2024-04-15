using ClassicalApi.Blazor.Client;
using ClassicalApi.Blazor.Client.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Radzen;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddRadzenComponents();
builder.Services.AddScoped<IComposerService, ComposerClientService>();
var ApiUrl = builder.Configuration.GetValue<string>("ApiService:Url") ?? "";
var ApiKey = builder.Configuration.GetValue<string>("ApiService:ApiKey");
builder.Services.AddHttpClient("ApiServer").ConfigureHttpClient(opt =>
{
    opt.BaseAddress = new Uri(ApiUrl);
    opt.DefaultRequestHeaders.Add("x-api-key", ApiKey);
});

builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddSingleton<AuthenticationStateProvider, PersistentAuthenticationStateProvider>();

await builder.Build().RunAsync();