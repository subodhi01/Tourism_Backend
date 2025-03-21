using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TourismGalle.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, NotMapped]
        public string Password { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        public string Role { get; set; } = "User";

        public string? ResetToken { get; set; } // Token for password reset
        public DateTime? ResetTokenExpiry { get; set; } // Expiry time for reset token
    }

}