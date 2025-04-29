using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using TourismGalle.Models;
using TourismGalle.Data;
using System;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace TourismGalle.Services
{
    public class AuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly EmailService _emailService;

        public AuthService(ApplicationDbContext context, EmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        public async Task<bool> Register(User user)
        {
            // Check if email exists in Users or PendingRegistrations
            var emailExistsInUsers = await _context.Users.AnyAsync(u => u.Email == user.Email);
            var emailExistsInPending = await _context.PendingRegistrations.AnyAsync(p => p.Email == user.Email);
            if (emailExistsInUsers || emailExistsInPending)
                return false;

            // Hash the password
            user.PasswordHash = HashPassword(user.Password);

            // Generate OTP
            user.RegistrationOTP = GenerateOTP();
            user.RegistrationOTPExpiry = DateTime.UtcNow.AddMinutes(10);

            // Save to PendingRegistrations using stored procedure
            await _context.Database.ExecuteSqlInterpolatedAsync(
                $"EXEC RegisterPendingUser @FullName={user.FullName}, @Email={user.Email}, @TelephoneNumber={user.TelephoneNumber}, @PasswordHash={user.PasswordHash}, @Role={user.Role}, @RegistrationOTP={user.RegistrationOTP}, @RegistrationOTPExpiry={user.RegistrationOTPExpiry}"
            );

            await _emailService.SendRegistrationOTPEmail(user.Email, user.RegistrationOTP);

            return true;
        }

        public async Task<bool> VerifyEmailOTP(string email, string otp)
        {
            var pending = await _context.PendingRegistrations
                .FirstOrDefaultAsync(p => p.Email == email && p.RegistrationOTP == otp && p.RegistrationOTPExpiry > DateTime.UtcNow);

            if (pending == null)
                return false;

            // Move to Users using RegisterUser stored procedure
            await _context.Database.ExecuteSqlInterpolatedAsync(
                $"EXEC RegisterUser @FullName={pending.FullName}, @Email={pending.Email}, @TelephoneNumber={pending.TelephoneNumber}, @PasswordHash={pending.PasswordHash}, @Role={pending.Role}, @RegistrationOTP={null}, @RegistrationOTPExpiry={null}"
            );

            // Remove from PendingRegistrations
            _context.PendingRegistrations.Remove(pending);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ResendOTP(string email)
        {
            var pending = await _context.PendingRegistrations.FirstOrDefaultAsync(p => p.Email == email);
            if (pending == null) return false;

            pending.RegistrationOTP = GenerateOTP();
            pending.RegistrationOTPExpiry = DateTime.UtcNow.AddMinutes(10);

            await _context.SaveChangesAsync();
            await _emailService.SendRegistrationOTPEmail(email, pending.RegistrationOTP);

            return true;
        }

        public async Task<User?> Login(string email, string password)
        {
            var users = await _context.Users
                .FromSqlInterpolated($"EXEC GetUserByEmail @Email={email}")
                .ToListAsync();

            var user = users.FirstOrDefault();

            if (user == null || !VerifyPassword(password, user.PasswordHash))
                return null;

            if (!user.IsEmailVerified)
                return null;

            return user;
        }

        public async Task<bool> RequestPasswordReset(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null) return false;

            user.ResetToken = Guid.NewGuid().ToString();
            user.ResetTokenExpiry = DateTime.UtcNow.AddHours(1);
            await _context.SaveChangesAsync();

            await _emailService.SendPasswordResetEmail(user.Email, user.ResetToken);

            return true;
        }

        public async Task<bool> ResetPassword(string token, string newPassword)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.ResetToken == token && u.ResetTokenExpiry > DateTime.UtcNow);
            if (user == null) return false;

            user.PasswordHash = HashPassword(newPassword);
            user.ResetToken = null;
            user.ResetTokenExpiry = null;

            await _context.SaveChangesAsync();
            return true;
        }

        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        private bool VerifyPassword(string password, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(password, hash);
        }

        private string GenerateOTP()
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                var otpBytes = new byte[4];
                rng.GetBytes(otpBytes);
                uint number = BitConverter.ToUInt32(otpBytes, 0);
                return (number % 1000000).ToString("D6");
            }
        }
    }
}