using Microsoft.AspNetCore.Mvc;
using TourismGalle.Models;
using TourismGalle.Services;

namespace TourismGalle.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TravelPlaceController : ControllerBase
    {
        private readonly TravelPlaceRepository _repository;

        public TravelPlaceController(TravelPlaceRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TravelPlace>>> GetAllTravelPlaces()
        {
            var places = await _repository.GetAllTravelPlacesAsync();
            return Ok(places);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TravelPlace>> GetTravelPlaceById(int id)
        {
            var place = await _repository.GetTravelPlaceByIdAsync(id);
            if (place == null)
                return NotFound();
            return Ok(place);
        }

        [HttpPost]
        public async Task<ActionResult> AddTravelPlace(TravelPlace place)
        {
            try
            {
                // Facilities are now directly available in the place.Facilities property
                var id = await _repository.AddTravelPlaceAsync(place, place.Facilities);
                return CreatedAtAction(nameof(GetTravelPlaceById), new { id }, place);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateTravelPlace(int id, TravelPlace place)
        {
            if (id != place.Id)
                return BadRequest();

            try
            {
                await _repository.UpdateTravelPlaceAsync(place, place.Facilities);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTravelPlace(int id)
        {
            await _repository.DeleteTravelPlaceAsync(id);
            return NoContent();
        }
    }
} 