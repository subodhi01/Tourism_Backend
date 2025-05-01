using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TourismGalle.Models
{
    public class TravelPlace
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string OwnerName { get; set; }

        [Required]
        [EmailAddress]
        public string OwnerEmail { get; set; }

        [Required]
        [StringLength(100)]
        public string PlaceName { get; set; }

        [Required]
        [StringLength(1000)]
        public string Description { get; set; }

        [Required]
        [StringLength(500)]
        public string LocationLink { get; set; }

        // Store images as JSON array string
        public string Images { get; set; }

        // Booking information
        [Required]
        [StringLength(1000)]
        public string BookingInstructions { get; set; }

        // Optional discount notices
        [StringLength(500)]
        public string DiscountNotices { get; set; }

        [Required]
        [StringLength(200)]
        public string ContactInfo { get; set; }

        // Store booked dates as JSON array string
        public string BookedDates { get; set; }

        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation property for facilities
        [NotMapped]
        public List<TravelPlaceFacility> Facilities { get; set; } = new List<TravelPlaceFacility>();
    }
} 