using Blazored.SessionStorage;
using BlazorIdentity.Client;
using ClassicalApi.Web.Client.Services;
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
builder.Services.AddHttpClient("AuthServer").ConfigureHttpClient(opt =>
{
    opt.BaseAddress = new Uri("http://localhost:5100/");
});

builder.Services.AddSingleton<IAuthenticateService, AuthenticateService>();
builder.Services.AddBlazoredSessionStorageAsSingleton();

builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddSingleton<AuthenticationStateProvider, PersistentAuthenticationStateProvider>();


await builder.Build().RunAsync();
