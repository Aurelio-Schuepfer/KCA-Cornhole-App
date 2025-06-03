using KCA_AuthentificationAPI.Models;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity;
using MimeKit;

public class MailKitEmailSender : IEmailSender<AppUser>
{
    private readonly EmailSettings _settings;

    public MailKitEmailSender(EmailSettings settings)
    {
        _settings = settings;
    }

    public async Task SendConfirmationLinkAsync(AppUser user, string email, string confirmationLink)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(_settings.FromName, _settings.FromEmail));
        message.To.Add(new MailboxAddress(user.UserName, email));
        message.Subject = "Bitte bestätige deine E-Mail-Adresse";

        string html = $@"
        <div style='font-family:Segoe UI,sans-serif;background:#181c24;color:#fff;padding:2.5rem 1.5rem;border-radius:1.2rem;max-width:420px;margin:2rem auto;box-shadow:0 0 30px #0008;text-align:center;'>
            <img src='http://localhost:4200/assets/images/CornholeLogoDarkMode.png' alt='Cornhole Logo' style='height:70px;margin-bottom:1.5rem;border-radius:1rem;'/>
            <h2 style='color:#ffb300;margin-bottom:1rem;'>E-Mail bestätigen</h2>
            <p style='font-size:1.1rem;margin-bottom:2rem;'>Hallo <b>{user.UserName}</b>,<br>klicke auf den Button, um deine E-Mail-Adresse zu bestätigen.</p>
            <a href='{confirmationLink}' style='display:inline-block;padding:1rem 2.2rem;background:#ffb300;color:#181c24;text-decoration:none;border-radius:2rem;font-weight:bold;font-size:1.1rem;margin-bottom:1.5rem;'>E-Mail bestätigen</a>
            <p style='font-size:0.95rem;color:#bbb;margin-top:2rem;'>Falls du kein Konto erstellt hast, ignoriere diese Mail.</p>
            <p style='font-size:0.9rem;color:#bbb;margin-top:1.5rem;'>Falls der Button nicht funktioniert, kopiere diesen Link in deinen Browser:<br>
            <a href='{confirmationLink}' style='color:#ffb300;'>{confirmationLink}</a></p>
        </div>";

        var builder = new BodyBuilder();
        builder.HtmlBody = html;

        message.Body = builder.ToMessageBody();

        await SendEmailAsync(message);
    }

    public async Task SendPasswordResetLinkAsync(AppUser user, string email, string resetLink)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(_settings.FromName, _settings.FromEmail));
        message.To.Add(new MailboxAddress(user.UserName, email));
        message.Subject = "KCA Cornhole – Passwort zurücksetzen";

        var html = $@"
        <div style='font-family:Segoe UI,sans-serif;background:#181c24;color:#fff;padding:2.5rem 1.5rem;border-radius:1.2rem;max-width:420px;margin:2rem auto;box-shadow:0 0 30px #0008;text-align:center;'>
            <img src='http://localhost:4200/assets/images/CornholeLogoDarkMode.png' alt='Cornhole Logo' style='height:70px;margin-bottom:1.5rem;border-radius:1rem;'/>
            <h2 style='color:#ffb300;margin-bottom:1rem;'>Passwort zurücksetzen</h2>
            <p style='font-size:1.1rem;margin-bottom:2rem;'>Hallo <b>{user.UserName}</b>,<br>klicke auf den Button, um dein Passwort zurückzusetzen.</p>
            <a href='{resetLink}' style='display:inline-block;padding:1rem 2.2rem;background:#ffb300;color:#181c24;text-decoration:none;border-radius:2rem;font-weight:bold;font-size:1.1rem;margin-bottom:1.5rem;'>Passwort zurücksetzen</a>
            <p style='font-size:0.95rem;color:#bbb;margin-top:2rem;'>Falls du kein Passwort-Reset angefordert hast, ignoriere diese Mail.</p>
            <p style='font-size:0.9rem;color:#bbb;margin-top:1.5rem;'>Falls der Button nicht funktioniert, kopiere diesen Link in deinen Browser:<br>
            <a href='{resetLink}' style='color:#ffb300;'>{resetLink}</a></p>
        </div>";

        message.Body = new TextPart("html") { Text = html };

        await SendEmailAsync(message);
    }

    public async Task SendPasswordResetCodeAsync(AppUser user, string email, string resetCode)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(_settings.FromName, _settings.FromEmail));
        message.To.Add(new MailboxAddress(user.UserName, email));
        message.Subject = "Your password reset code";

        message.Body = new TextPart("plain")
        {
            Text = $"Use this code to reset your password: {resetCode}"
        };

        await SendEmailAsync(message);
    }

    private async Task SendEmailAsync(MimeMessage message)
    {
        using var client = new SmtpClient();
        await client.ConnectAsync(_settings.SmtpServer, _settings.SmtpPort, _settings.UseSSL);
        if (!string.IsNullOrEmpty(_settings.SmtpUser))
            await client.AuthenticateAsync(_settings.SmtpUser, _settings.SmtpPass);

        await client.SendAsync(message);
        await client.DisconnectAsync(true);
    }
}
