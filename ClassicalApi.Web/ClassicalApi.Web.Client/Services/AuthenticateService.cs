using Blazored.SessionStorage;
using ClassicalApi.Web.Client.Models;
using System.Net.Http.Json;

namespace ClassicalApi.Web.Client.Services;

public class AuthenticateService : IAuthenticateService
{
    private const string JWT_KEY = nameof(JWT_KEY);

    private readonly HttpClient _httpClient;
    private readonly ISessionStorageService _sessionStorageService;
    private string? _jwtCached;

    public AuthenticateService(IHttpClientFactory clientFactory, ISessionStorageService sessionStorage)
    {
        _httpClient = clientFactory.CreateClient("AuthServer");
        _sessionStorageService = sessionStorage;
    }

    public async ValueTask<string?> GetJwtAsync()
    {
        _jwtCached ??= await _sessionStorageService.GetItemAsync<string>(JWT_KEY);
        return _jwtCached;
    }

    public async Task<LoginResult> LoginAsync(LoginModel loginData)
    {
        var response = await _httpClient.PostAsJsonAsync("/login?useCookies=true", loginData);
        if(response == null || !response.IsSuccessStatusCode)
        {
            return LoginResult.Failure;
        }

        //var result = await response.Content.ReadFromJsonAsync<LoginResult>() ?? new();

        //await _sessionStorageService.SetItemAsync(JWT_KEY, result.AccessToken);

        return new LoginResult() { IsSucceeded = true };
    }

    public async Task LogoutAsync()
    {
        await _sessionStorageService.RemoveItemAsync(JWT_KEY);
        _jwtCached = null;
    }

    public Task<RegisterResult> RegisterAsync(RegisterModel registerData)
    {
        throw new NotImplementedException();
    }
}
