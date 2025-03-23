using System.ComponentModel.DataAnnotations;

namespace TourismGalle.Models
{
    public class Place
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public string Images { get; set; } // Store image URLs as a string (comma-separated)

        public decimal Price { get; set; }

        public string Packages { get; set; }

        public int Capacity { get; set; }

        public string TimeSlots { get; set; } // Example: "10:00 AM - 6:00 PM"

        public string SpecialFunctions { get; set; } // Example: "Weddings, Conferences"

        public string Location { get; set; } // Google Maps link or address

        public string ContactInfo { get; set; }

        public bool IsActive { get; set; } = true; // If the place is available
    }
}
