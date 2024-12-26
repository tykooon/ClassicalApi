using ClassicalApi.Blazor.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace ClassicalApi.Blazor.Components.Account
{
    public class IdentityAppEmailSender : IEmailSender<AppUser>
    {
        private readonly IEmailSender emailSender;

        public IdentityAppEmailSender(IEmailSender sender)
        {
            emailSender = sender;
        }

        public Task SendConfirmationLinkAsync(AppUser user, string email, string confirmationLink) =>
            emailSender.SendEmailAsync(email, "Confirm your email", $"Please confirm your account by <a href='{confirmationLink}'>clicking here</a>.");

        public Task SendPasswordResetLinkAsync(AppUser user, string email, string resetLink) =>
            emailSender.SendEmailAsync(email, "Reset your password", $"Please reset your password by <a href='{resetLink}'>clicking here</a>.");

        public Task SendPasswordResetCodeAsync(AppUser user, string email, string resetCode) =>
            emailSender.SendEmailAsync(email, "Reset your password", $"Please reset your password using the following code: {resetCode}");
    }
}