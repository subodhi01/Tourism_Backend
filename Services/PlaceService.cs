using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using TourismGalle.Models;
using TourismGalle.Data;

namespace TourismGalle.Services
{
    public class PlaceService
    {
        private readonly ApplicationDbContext _context;

        public PlaceService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Place>> GetAllPlaces()
        {
            return await _context.Places.ToListAsync();
        }

        public async Task<Place> GetPlaceById(int id)
        {
            return await _context.Places.FindAsync(id);
        }

        public async Task AddPlace(Place place)
        {
            place.CreatedAt = DateTime.UtcNow;
            place.UpdatedAt = DateTime.UtcNow;
            _context.Places.Add(place);
            await _context.SaveChangesAsync();
        }

        public async Task UpdatePlace(Place place)
        {
            place.UpdatedAt = DateTime.UtcNow;
            _context.Places.Update(place);
            await _context.SaveChangesAsync();
        }

        public async Task DeletePlace(int id)
        {
            var place = await _context.Places.FindAsync(id);
            if (place != null)
            {
                _context.Places.Remove(place);
                await _context.SaveChangesAsync();
            }
        }
    }
}
