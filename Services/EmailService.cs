using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

public class EmailService
{
    private readonly IConfiguration _configuration;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendPasswordResetEmail(string email, string token)
    {
        string resetLink = $"https://yourfrontend.com/reset-password?token={token}";
        string subject = "Password Reset Request";
        string body = $"Click the link below to reset your password:\n\n{resetLink}";

        await SendEmailAsync(email, subject, body);
    }

    public async Task SendRegistrationOTPEmail(string email, string otp)
    {
        string subject = "Email Verification OTP";
        string body = $"Your verification code is: {otp}\n\nThis code will expire in 10 minutes.\n\nIf you didn't request this code, please ignore this email.";

        await SendEmailAsync(email, subject, body);
    }

    private async Task SendEmailAsync(string toEmail, string subject, string body)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("Tourism Galle", _configuration["EmailSettings:SenderEmail"]));
        message.To.Add(new MailboxAddress(toEmail, toEmail));
        message.Subject = subject;

        var bodyBuilder = new BodyBuilder { TextBody = body };
        message.Body = bodyBuilder.ToMessageBody();

        using (var smtp = new SmtpClient())
        {
            await smtp.ConnectAsync(_configuration["EmailSettings:SmtpServer"],
                                    int.Parse(_configuration["EmailSettings:SmtpPort"]), SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_configuration["EmailSettings:SmtpUsername"],
                                         _configuration["EmailSettings:SmtpPassword"]);
            await smtp.SendAsync(message);
            await smtp.DisconnectAsync(true);
        }

        Console.WriteLine($"Email sent to {toEmail}");
    }
}
