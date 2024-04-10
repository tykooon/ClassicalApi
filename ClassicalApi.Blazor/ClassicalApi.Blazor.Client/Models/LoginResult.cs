namespace ClassicalApi.Blazor.Client.Models;

public class LoginResult
{
    public static readonly LoginResult Failure = new() { IsSucceeded = false };

    public bool IsSucceeded { get; set; } = false;

    public string[] Errors { get; set; } = [];

    public string? TokenType { get; set; }

    public int ExpiresIn { get; set; }

    public string? AccessToken { get; set; }

    public string? RefreshToken { get; set; }
}
