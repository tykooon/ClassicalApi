using ClassicalApi.Blazor.Client.Models;

namespace ClassicalApi.Blazor.Client.Services;

public interface IAuthenticateService
{
    Task<LoginResult> LoginAsync(LoginModel loginData);
    Task<RegisterResult> RegisterAsync(RegisterModel registerData);
    Task LogoutAsync();
}
