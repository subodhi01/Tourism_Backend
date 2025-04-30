using Microsoft.AspNetCore.Mvc;
using TourismGalle.Data;
using TourismGalle.Models;
using TourismGalle.Services;
using System.Threading.Tasks;

namespace TourismGalle.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var user = new User
                {
                    FullName = request.FullName,
                    Email = request.Email,
                    TelephoneNumber = request.TelephoneNumber,
                    Password = request.Password,
                    Role = request.Role
                };

                var result = await _authService.Register(user);
                if (!result)
                {
                    return BadRequest(new { Message = "Registration failed: Email already exists." });
                }

                return Ok(new { Message = "Registration pending. Please verify OTP." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Registration failed", Error = ex.Message });
            }
        }

        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOtp([FromBody] VerifyOtpRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _authService.VerifyEmailOTP(request.Email, request.Otp);
                if (!result)
                {
                    return BadRequest(new { Message = "OTP verification failed: Invalid or expired OTP." });
                }

                return Ok(new { Message = "OTP verified successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "OTP verification failed", Error = ex.Message });
            }
        }

        [HttpPost("resend-otp")]
        public async Task<IActionResult> ResendOtp([FromBody] ResendOtpRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _authService.ResendOTP(request.Email);
                if (!result)
                {
                    return BadRequest(new { Message = "Failed to resend OTP: Email not found." });
                }

                return Ok(new { Message = "OTP resent successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Failed to resend OTP", Error = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var user = await _authService.Login(request.Email, request.Password);
                if (user == null)
                {
                    return Unauthorized(new { Message = "Invalid email or password, or email not verified." });
                }

                return Ok(new
                {
                    user = new
                    {
                        id = user.Id.ToString(),
                        fullName = user.FullName,
                        email = user.Email,
                        telephone = user.TelephoneNumber,
                        profilePhoto = user.ProfilePhoto,
                        role = user.Role,
                        isEmailVerified = user.IsEmailVerified
                    }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Login failed", Error = ex.Message });
            }
        }

        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile([FromQuery] string email)
        {
            try
            {
                var user = await _authService.GetUserByEmail(email);
                if (user == null)
                {
                    return NotFound(new { Message = "User not found" });
                }

                return Ok(new
                {
                    user = new
                    {
                        id = user.Id.ToString(),
                        fullName = user.FullName,
                        email = user.Email,
                        telephone = user.TelephoneNumber,
                        profilePhoto = user.ProfilePhoto,
                        role = user.Role,
                        isEmailVerified = user.IsEmailVerified
                    }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Failed to get profile", Error = ex.Message });
            }
        }

        public class LoginRequest
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }

        public class VerifyOtpRequest
        {
            public string Email { get; set; }
            public string Otp { get; set; }
        }

        public class ResendOtpRequest
        {
            public string Email { get; set; }
        }
    }
}