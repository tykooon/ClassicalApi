using ClassicalApi.Web.Client.Models;

namespace ClassicalApi.Web.Client.Services;

public interface IAuthenticateService
{
    Task<LoginResult> LoginAsync(LoginModel loginData);
    Task<RegisterResult> RegisterAsync(RegisterModel registerData);
    Task LogoutAsync();
}
