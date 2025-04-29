using System.ComponentModel.DataAnnotations;

namespace TourismGalle.Models
{
    public class RegisterRequest
    {
        [Required]
        public string FullName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string TelephoneNumber { get; set; }

        [Required]
        public string Password { get; set; }

        public string Role { get; set; } = "User";
    }
}
