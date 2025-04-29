using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TourismGalle.Models
{
    //public class User
    //{
    //    [Key]
    //    public int Id { get; set; }

    //    [Required]
    //    public string FullName { get; set; }

    //    [Required, EmailAddress]
    //    public string Email { get; set; }

    //    [Required]
    //    public string TelephoneNumber { get; set; }

    //    [Required, NotMapped]
    //    public string Password { get; set; }

    //    [Required]
    //    public string PasswordHash { get; set; }

    //    public string Role { get; set; } = "User";

    //    public string? ResetToken { get; set; }
    //    public DateTime? ResetTokenExpiry { get; set; }

    //    public string? RegistrationOTP { get; set; }
    //    public DateTime? RegistrationOTPExpiry { get; set; }
    //    public bool IsEmailVerified { get; set; } = false;
    //}

    //    public class User
    //    {
    //        [Key]
    //        public int Id { get; set; }

    //        [Required]
    //        public string FullName { get; set; }

    //        [Required, EmailAddress]
    //        public string Email { get; set; }

    //        [Required]
    //        public string TelephoneNumber { get; set; }

    //        [Required, NotMapped]
    //        public string Password { get; set; }

    //        [Required]
    //        public string PasswordHash { get; set; }

    //        public string Role { get; set; } = "User";

    //        public string? ResetToken { get; set; }
    //        public DateTime? ResetTokenExpiry { get; set; }

    //        public string? RegistrationOTP { get; set; }
    //        public DateTime? RegistrationOTPExpiry { get; set; }
    //        public bool IsEmailVerified { get; set; } = false;

    //        public string? ProfilePhoto { get; set; } // Added for frontend
    //    }
    //}
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string FullName { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; }
        [Required]
        public string TelephoneNumber { get; set; }
        [Required, NotMapped]
        public string Password { get; set; }
        [Required]
        public string PasswordHash { get; set; }
        public string Role { get; set; } = "User";
        public string? ResetToken { get; set; }
        public DateTime? ResetTokenExpiry { get; set; }
        public string? RegistrationOTP { get; set; }
        public DateTime? RegistrationOTPExpiry { get; set; }
        public bool IsEmailVerified { get; set; } = false;
        public string? ProfilePhoto { get; set; }
    }
}