using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using TourismGalle.Models;
using System;
using System.Threading.Tasks;

public class AuthService
{
    private readonly ApplicationDbContext _context;
    private readonly EmailService _emailService;

    public AuthService(ApplicationDbContext context, EmailService emailService)
    {
        _context = context;
        _emailService = emailService;
    }

    // ✅ Register (Signup) using Stored Procedure
    public async Task<bool> Register(User user)
    {
        // Check if email already exists
        var emailExists = await _context.Users.AnyAsync(u => u.Email == user.Email);
        if (emailExists)
            return false; // Email already exists

        // Hash the password
        user.PasswordHash = HashPassword(user.Password);

        // Call stored procedure for registration
        await _context.Database.ExecuteSqlInterpolatedAsync(
            $"EXEC RegisterUser @FullName={user.FullName}, @Email={user.Email}, @PasswordHash={user.PasswordHash}, @Role={user.Role}"
        );

        return true;
    }

    // ✅ Login using Stored Procedure
    public async Task<User?> Login(string email, string password)
    {
        var users = await _context.Users
            .FromSqlInterpolated($"EXEC GetUserByEmail @Email={email}")
            .ToListAsync();

        var user = users.FirstOrDefault();

        if (user == null || !VerifyPassword(password, user.PasswordHash))
            return null;

        return user;
    }

    // ✅ Request Password Reset
    public async Task<bool> RequestPasswordReset(string email)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        if (user == null) return false; // User not found

        // Generate reset token
        user.ResetToken = Guid.NewGuid().ToString();
        user.ResetTokenExpiry = DateTime.UtcNow.AddHours(1); // Token valid for 1 hour
        await _context.SaveChangesAsync();

        // Send reset email
        await _emailService.SendPasswordResetEmail(user.Email, user.ResetToken);

        return true;
    }

    // ✅ Reset Password
    public async Task<bool> ResetPassword(string token, string newPassword)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.ResetToken == token && u.ResetTokenExpiry > DateTime.UtcNow);
        if (user == null) return false; // Invalid or expired token

        // Hash the new password and update
        user.PasswordHash = HashPassword(newPassword);
        user.ResetToken = null; // Clear token after use
        user.ResetTokenExpiry = null;

        await _context.SaveChangesAsync();
        return true;
    }

    // ✅ Password Hashing using BCrypt
    private string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    private bool VerifyPassword(string password, string hash)
    {
        return BCrypt.Net.BCrypt.Verify(password, hash);
    }
}
