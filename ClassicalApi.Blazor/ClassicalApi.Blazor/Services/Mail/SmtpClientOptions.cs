namespace ClassicalApi.Blazor.Services.Mail;

public class SmtpClientOptions
{
    public string Host { get; set; } = "example.com";
    public int Port { get; set; } = 25;
    public bool EnableSsl { get; set; }
    public string Username { get; set; } = "anonymous";
    public string Password { get; set; } = "";
    public bool UseDefaultCredentials { get; set; } = true;
}
