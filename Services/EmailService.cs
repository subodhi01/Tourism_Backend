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
        try
        {
            Console.WriteLine($"Attempting to send OTP email to: {email}");
            string subject = "Email Verification OTP";
            string body = $"Your verification code is: {otp}\n\nThis code will expire in 10 minutes.\n\nIf you didn't request this code, please ignore this email.";

            await SendEmailAsync(email, subject, body);
            Console.WriteLine($"OTP email sent successfully to: {email}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error sending OTP email: {ex.Message}");
            Console.WriteLine($"Stack trace: {ex.StackTrace}");
            throw; // Re-throw to handle in the calling method
        }
    }

    private async Task SendEmailAsync(string toEmail, string subject, string body)
    {
        try
        {
            Console.WriteLine($"Preparing to send email to: {toEmail}");
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_configuration["EmailSettings:SenderName"], _configuration["EmailSettings:SenderEmail"]));
            message.To.Add(new MailboxAddress(toEmail, toEmail));
            message.Subject = subject;

            var bodyBuilder = new BodyBuilder { TextBody = body };
            message.Body = bodyBuilder.ToMessageBody();

            Console.WriteLine($"Connecting to SMTP server: {_configuration["EmailSettings:SmtpServer"]}:{_configuration["EmailSettings:SmtpPort"]}");
            using (var smtp = new SmtpClient())
            {
                await smtp.ConnectAsync(_configuration["EmailSettings:SmtpServer"],
                                    int.Parse(_configuration["EmailSettings:SmtpPort"]), SecureSocketOptions.StartTls);
                
                Console.WriteLine("Authenticating with SMTP server...");
                await smtp.AuthenticateAsync(_configuration["EmailSettings:SmtpUsername"],
                                         _configuration["EmailSettings:SmtpPassword"]);
                
                Console.WriteLine("Sending email...");
                await smtp.SendAsync(message);
                
                Console.WriteLine("Disconnecting from SMTP server...");
                await smtp.DisconnectAsync(true);
            }

            Console.WriteLine($"Email sent successfully to {toEmail}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in SendEmailAsync: {ex.Message}");
            Console.WriteLine($"Stack trace: {ex.StackTrace}");
            throw;
        }
    }
}
