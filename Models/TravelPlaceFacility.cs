using System.ComponentModel.DataAnnotations;

namespace TourismGalle.Models
{
    public class TravelPlaceFacility
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int TravelPlaceId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(500)]
        public string Description { get; set; }

        [Required]
        public decimal AveragePrice { get; set; }

        [Required]
        public decimal PricePerPerson { get; set; }

        [Required]
        [StringLength(100)]
        public string Duration { get; set; }

        [Required]
        public string Availability { get; set; } // "Weekdays", "Weekend", "Both"

        [StringLength(500)]
        public string SpecialNotices { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
} 