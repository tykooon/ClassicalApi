using ClassicalApi.Blazor.Client;
using ClassicalApi.Blazor.Client.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Radzen;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddRadzenComponents();
builder.Services.AddScoped<IComposerService, ComposerClientService>();
builder.Services.AddHttpClient("ApiServer").ConfigureHttpClient(opt =>
{
    opt.BaseAddress = new Uri("http://localhost:5000");
    opt.DefaultRequestHeaders.Add("x-api-key", "muokY3cGjaA6juhmJmKyOOoZDhDscmrst2LosF9HieS5IH8o4JkkBroYEgqmn4yHVdXlqvpzm7Z5pn3iZqGJF5a8jL2SmcZzEHOEQpPeX1XermLkV6KImCybcDNQ3TVr");
});

builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddSingleton<AuthenticationStateProvider, PersistentAuthenticationStateProvider>();

await builder.Build().RunAsync();