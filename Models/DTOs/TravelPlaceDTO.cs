namespace TourismGalle.Models.DTOs
{
    public class TravelPlaceCreateRequest
    {
        public TravelPlace Place { get; set; }
        public List<TravelPlaceFacility> Facilities { get; set; }
    }
} 