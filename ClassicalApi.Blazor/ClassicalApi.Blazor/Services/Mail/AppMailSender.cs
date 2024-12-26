using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net;
using System.Net.Mail;

namespace ClassicalApi.Blazor.Services.Mail;

internal class AppMailSender : IEmailSender
{
    private SmtpClient? _client;
    private SmtpClientOptions? _options;
    private ILogger _logger;

    public AppMailSender(IConfiguration configuration, ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger(nameof(AppMailSender));
        _options = configuration.GetSection("SmtpSettings").Get<SmtpClientOptions>();

        if (_options is null)
        {
            _logger.LogCritical("SmtpSettings configuration is missing.");
            _options = new SmtpClientOptions();
        }

        _client = new(_options.Host, 587);
        _client.EnableSsl = true;
        _client.UseDefaultCredentials = false;
        _client.Credentials = new NetworkCredential(_options.Username, _options.Password);
    }

    public Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        var mailMessage = new MailMessage
        {
            From = new MailAddress(_options?.Username ?? "")
        };
        mailMessage.To.Add(email);
        mailMessage.Subject = subject;
        mailMessage.IsBodyHtml = true;
        mailMessage.Body = htmlMessage;

        // Send email
        return _client?.SendMailAsync(mailMessage) ?? Task.CompletedTask;
    }
}