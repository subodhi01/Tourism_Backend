using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using TourismGalle.Models;
using TourismGalle.Services;
using TourismGalle.Data;

namespace TourismGalle.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        // ✅ Register API
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] User user)
        {
            try
            {
                Console.WriteLine($"Register request received for email: {user.Email}");
                Console.WriteLine($"Request data: {System.Text.Json.JsonSerializer.Serialize(user)}");
                
                bool isRegistered = await _authService.Register(user);
                if (!isRegistered)
                {
                    Console.WriteLine("Registration failed: Email already exists");
                    return BadRequest("Email already exists");
                }

                Console.WriteLine("Registration successful");
                return Ok(new { message = "Registration successful. Please check your email for OTP verification." });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Registration error: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                return BadRequest($"Registration failed: {ex.Message}");
            }
        }

        // ✅ Verify Email OTP
        [HttpPost("verify-email")]
        public async Task<IActionResult> VerifyEmail([FromBody] EmailVerificationRequest request)
        {
            var result = await _authService.VerifyEmailOTP(request.Email, request.OTP);
            if (!result)
                return BadRequest("Invalid or expired OTP");

            return Ok(new { message = "Email verified successfully" });
        }

        // ✅ Resend OTP
        [HttpPost("resend-otp")]
        public async Task<IActionResult> ResendOTP([FromBody] ResendOTPRequest request)
        {
            var result = await _authService.ResendOTP(request.Email);
            if (!result)
                return BadRequest("User not found");

            return Ok(new { message = "OTP resent successfully" });
        }

        // ✅ Login API
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var authenticatedUser = await _authService.Login(request.Email, request.Password);
            if (authenticatedUser == null)
                return Unauthorized("Invalid email or password, or email not verified");

            return Ok(new { message = "Login Successful", user = authenticatedUser });
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
        {
            bool result = await _authService.RequestPasswordReset(request.Email);
            if (!result)
                return BadRequest("User not found.");

            return Ok("Reset password link has been sent to your email.");
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            bool result = await _authService.ResetPassword(request.Token, request.NewPassword);
            if (!result)
                return BadRequest("Invalid or expired token.");

            return Ok("Password has been reset successfully.");
        }

        public class ForgotPasswordRequest
        {
            [Required, EmailAddress]
            public string Email { get; set; }
        }

        public class ResetPasswordRequest
        {
            [Required]
            public string Token { get; set; }

            [Required]
            public string NewPassword { get; set; }
        }

        public class LoginRequest
        {
            [Required, EmailAddress]
            public string Email { get; set; }

            [Required]
            public string Password { get; set; }
        }

        public class EmailVerificationRequest
        {
            [Required, EmailAddress]
            public string Email { get; set; }

            [Required, StringLength(6, MinimumLength = 6)]
            public string OTP { get; set; }
        }

        public class ResendOTPRequest
        {
            [Required, EmailAddress]
            public string Email { get; set; }
        }
    }
}