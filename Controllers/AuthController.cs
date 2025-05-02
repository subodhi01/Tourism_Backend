using Microsoft.AspNetCore.Mvc;
using TourismGalle.Data;
using TourismGalle.Models;
using TourismGalle.Services;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Linq;

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

        [HttpPut("profile")]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileRequest request)
        {
            Console.WriteLine($"Received profile update request for email: {request.Email}");
            Console.WriteLine($"New full name: {request.FullName}");
            Console.WriteLine($"New telephone: {request.TelephoneNumber}");

            if (!ModelState.IsValid)
            {
                Console.WriteLine("Model validation failed");
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine($"Validation error: {error.ErrorMessage}");
                }
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _authService.UpdateProfile(request.Email, request.FullName, request.TelephoneNumber);
                if (!result)
                {
                    Console.WriteLine("User not found for update");
                    return NotFound(new { Message = "User not found" });
                }

                Console.WriteLine("Profile updated successfully");
                return Ok(new { Message = "Profile updated successfully" });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating profile: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                return StatusCode(500, new { Message = "Failed to update profile", Error = ex.Message });
            }
        }

        [HttpPost("forget-password")]
        public async Task<IActionResult> ForgetPassword([FromBody] ForgetPasswordRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _authService.RequestPasswordReset(request.Email);
                if (!result)
                {
                    return BadRequest(new { Message = "Email not found." });
                }

                return Ok(new { Message = "Password reset OTP sent successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Failed to send password reset OTP", Error = ex.Message });
            }
        }

        [HttpPost("verify-reset-otp")]
        public async Task<IActionResult> VerifyResetOtp([FromBody] VerifyResetOtpRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _authService.VerifyResetOTP(request.Email, request.Otp);
                if (!result)
                {
                    return BadRequest(new { Message = "Invalid or expired OTP." });
                }

                return Ok(new { Message = "OTP verified successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Failed to verify OTP", Error = ex.Message });
            }
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _authService.ResetPassword(request.Email, request.Otp, request.NewPassword);
                if (!result)
                {
                    return BadRequest(new { Message = "Failed to reset password: Invalid OTP or email." });
                }

                return Ok(new { Message = "Password reset successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Failed to reset password", Error = ex.Message });
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

        public class UpdateProfileRequest
        {
            [Required, EmailAddress]
            public string Email { get; set; }

            [Required]
            public string FullName { get; set; }

            [Required]
            public string TelephoneNumber { get; set; }
        }

        public class ForgetPasswordRequest
        {
            [Required, EmailAddress]
            public string Email { get; set; }
        }

        public class VerifyResetOtpRequest
        {
            [Required, EmailAddress]
            public string Email { get; set; }

            [Required]
            public string Otp { get; set; }
        }

        public class ResetPasswordRequest
        {
            [Required, EmailAddress]
            public string Email { get; set; }

            [Required]
            public string Otp { get; set; }

            [Required]
            public string NewPassword { get; set; }
        }

        public class ResetPasswordLoggedInRequest
        {
            [Required, EmailAddress]
            public string Email { get; set; }
            [Required]
            public string CurrentPassword { get; set; }
            [Required]
            public string NewPassword { get; set; }
            [Required]
            public string ConfirmNewPassword { get; set; }
        }

        public class DeleteAccountRequest
        {
            [Required, EmailAddress]
            public string Email { get; set; }
            [Required]
            public string Password { get; set; }
        }

        [HttpPost("reset-password-logged-in")]
        public async Task<IActionResult> ResetPasswordLoggedIn([FromBody] ResetPasswordLoggedInRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _authService.ResetPasswordForLoggedInUser(request.Email, request.CurrentPassword, request.NewPassword);
                if (!result)
                {
                    return BadRequest(new { Message = "Failed to reset password: Invalid current password or email." });
                }

                return Ok(new { 
                    Message = "Password reset successfully. You will be automatically logged out. Please login again with your new password.",
                    ShouldLogout = true 
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Failed to reset password", Error = ex.Message });
            }
        }

        [HttpDelete("account")]
        public async Task<IActionResult> DeleteAccount([FromBody] DeleteAccountRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _authService.DeleteAccount(request.Email, request.Password);
                if (!result)
                {
                    return BadRequest(new { Message = "Failed to delete account: Invalid email or password." });
                }

                return Ok(new { Message = "Account deleted successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Failed to delete account", Error = ex.Message });
            }
        }

        [HttpGet("users")]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                Console.WriteLine("Attempting to get all users...");
                var users = await _authService.GetAllUsers();
                Console.WriteLine($"Retrieved {users.Count} users");

                var response = new
                {
                    users = users.Select(u => new
                    {
                        id = u.Id,
                        fullName = u.FullName,
                        email = u.Email,
                        telephone = u.TelephoneNumber,
                        role = u.Role,
                        isEmailVerified = u.IsEmailVerified,
                        profilePhoto = u.ProfilePhoto
                    })
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetAllUsers endpoint: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                }
                return StatusCode(500, new { 
                    Message = "Failed to get users", 
                    Error = ex.Message,
                    StackTrace = ex.StackTrace,
                    InnerException = ex.InnerException?.Message
                });
            }
        }

        public class UpdateUserRequest
        {
            [Required]
            public int Id { get; set; }

            [Required]
            public string FullName { get; set; }

            [Required, EmailAddress]
            public string Email { get; set; }

            [Required]
            public string TelephoneNumber { get; set; }

            [Required]
            public string Role { get; set; }
        }

        [HttpPut("users/{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _authService.UpdateUser(id, request.FullName, request.Email, request.TelephoneNumber, request.Role);
                if (!result)
                {
                    return NotFound(new { Message = "User not found or update failed" });
                }

                return Ok(new { Message = "User updated successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Failed to update user", Error = ex.Message });
            }
        }

        [HttpDelete("users/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                var result = await _authService.DeleteUser(id);
                if (!result)
                {
                    return NotFound(new { Message = "User not found or delete failed" });
                }

                return Ok(new { Message = "User deleted successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Failed to delete user", Error = ex.Message });
            }
        }

        public class CreateUserRequest
        {
            [Required]
            public string FullName { get; set; }

            [Required, EmailAddress]
            public string Email { get; set; }

            [Required]
            public string TelephoneNumber { get; set; }

            [Required]
            public string Role { get; set; }

            [Required]
            public string Password { get; set; }
        }

        [HttpPost("users")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request)
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
                    Role = request.Role,
                    IsEmailVerified = true // Since admin is creating the user
                };

                var result = await _authService.CreateUser(user);
                if (!result)
                {
                    return BadRequest(new { Message = "Failed to create user: Email already exists." });
                }

                return Ok(new { Message = "User created successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Failed to create user", Error = ex.Message });
            }
        }
    }
}