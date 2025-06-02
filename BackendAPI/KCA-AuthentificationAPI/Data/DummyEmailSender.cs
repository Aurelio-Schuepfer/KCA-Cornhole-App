using KCA_AuthentificationAPI.Models;
using Microsoft.AspNetCore.Identity;

namespace KCA_AuthentificationAPI.Data
{
    public class DummyEmailSender : IEmailSender<AppUser>
    {
        public Task SendConfirmationLinkAsync(AppUser user, string email, string confirmationLink)
            => Task.CompletedTask;

        public Task SendPasswordResetLinkAsync(AppUser user, string email, string resetLink)
            => Task.CompletedTask;

        public Task SendPasswordResetCodeAsync(AppUser user, string email, string resetCode)
            => Task.CompletedTask;
    }
}
