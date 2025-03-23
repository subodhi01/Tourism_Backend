using System.ComponentModel.DataAnnotations;

namespace TourismGalle.Models
{
    public class Place
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [StringLength(1000)]
        public string Images { get; set; } // Store image URLs as a string (comma-separated)

        [Required]
        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }

        [StringLength(500)]
        public string Packages { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int Capacity { get; set; }

        [StringLength(100)]
        public string TimeSlots { get; set; } // Example: "10:00 AM - 6:00 PM"

        [StringLength(200)]
        public string SpecialFunctions { get; set; } // Example: "Weddings, Conferences"

        [Required]
        [StringLength(200)]
        public string Location { get; set; } // Google Maps link or address

        [Required]
        [StringLength(100)]
        public string ContactInfo { get; set; }

        public bool IsActive { get; set; } = true; // If the place is available

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
