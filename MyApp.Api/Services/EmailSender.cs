using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Identity;
using MimeKit;
using MyApp.Api.Data;

namespace MyApp.Api.Services;

public class EmailSender : IEmailSender<ApplicationUser>
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<EmailSender> _logger;

    public EmailSender(IConfiguration configuration, ILogger<EmailSender> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public async Task SendConfirmationLinkAsync(ApplicationUser user, string email, string confirmationLink)
    {
        var subject = "Confirm your email";
        var body = $"""
            <h2>Welcome to MyApp!</h2>
            <p>Please confirm your email address by clicking the link below:</p>
            <p><a href="{confirmationLink}" style="padding:10px 20px;background:#4F46E5;color:white;border-radius:6px;text-decoration:none;">Confirm Email</a></p>
            <p>If you did not create an account, you can safely ignore this email.</p>
            """;

        await SendEmailAsync(email, subject, body);
    }

    public async Task SendPasswordResetCodeAsync(ApplicationUser user, string email, string resetCode)
    {
        var subject = "Reset your password";
        var body = $"""
            <h2>Password Reset</h2>
            <p>Use the following code to reset your password:</p>
            <h1 style="letter-spacing:8px;color:#4F46E5;">{resetCode}</h1>
            <p>This code expires in 15 minutes. If you did not request a reset, ignore this email.</p>
            """;

        await SendEmailAsync(email, subject, body);
    }

    public async Task SendPasswordResetLinkAsync(ApplicationUser user, string email, string resetLink)
    {
        var subject = "Reset your password";
        var body = $"""
            <h2>Password Reset</h2>
            <p>Click the link below to reset your password:</p>
            <p><a href="{resetLink}" style="padding:10px 20px;background:#4F46E5;color:white;border-radius:6px;text-decoration:none;">Reset Password</a></p>
            <p>If you did not request a reset, ignore this email.</p>
            """;

        await SendEmailAsync(email, subject, body);
    }

    private async Task SendEmailAsync(string toEmail, string subject, string htmlBody)
    {
        var username = _configuration["Email:Username"];
        var password = _configuration["Email:Password"];

        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        {
            _logger.LogError("Email credentials are not configured. Set Email:Username and Email:Password in User Secrets.");
            throw new InvalidOperationException("Email service is not configured.");
        }

        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("MyApp", username));
        message.To.Add(MailboxAddress.Parse(toEmail));
        message.Subject = subject;
        message.Body = new TextPart("html") { Text = htmlBody };

        using var smtp = new SmtpClient();
        try
        {
            await smtp.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(username, password);
            await smtp.SendAsync(message);
            await smtp.DisconnectAsync(true);

            _logger.LogInformation("Email '{Subject}' sent to {Email}", subject, toEmail);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send email '{Subject}' to {Email}", subject, toEmail);
            throw;
        }
    }
}
