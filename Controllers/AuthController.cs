using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using TourismGalle.Models;

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
        Console.WriteLine("Register request received for email: " + user.Email); // Add this line
        bool isRegistered = await _authService.Register(user);
        if (!isRegistered)
            return BadRequest("Email already exists");

        return Ok("User registered successfully");
    }

    // ✅ Login API
    [HttpPost("login")]
  
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var authenticatedUser = await _authService.Login(request.Email, request.Password);
        if (authenticatedUser == null)
            return Unauthorized("Invalid email or password");

        return Ok("Login Successfully");
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



    // Login Request DTO
    public class LoginRequest
    {
        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }

}