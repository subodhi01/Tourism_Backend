using System;
using System.ComponentModel.DataAnnotations;

namespace TourismGalle.Models
{
    public class PendingRegistration
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string TelephoneNumber { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        public string Role { get; set; } = "User";

        public string RegistrationOTP { get; set; }

        public DateTime? RegistrationOTPExpiry { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime ExpiresAt { get; set; }
    }
}