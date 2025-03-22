namespace TourismGalle.Models
{
    public class TourPackage
    {
        public int PackageID { get; set; }
        public string PackageName { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int DurationDays { get; set; }
        public string Place { get; set; } // New field for Place
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
