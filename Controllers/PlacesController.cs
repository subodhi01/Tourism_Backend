using Microsoft.AspNetCore.Mvc;
using TourismGalle.Models;
using TourismGalle.Services;

namespace TourismGalle.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlacesController : ControllerBase
    {
        private readonly PlacesRepository _repository;

        public PlacesController(PlacesRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Place>>> GetAllPlaces()
        {
            var places = await _repository.GetAllPlacesAsync();
            return Ok(places);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Place>> GetPlaceById(int id)
        {
            var place = await _repository.GetPlaceByIdAsync(id);
            if (place == null)
                return NotFound();
            return Ok(place);
        }

        [HttpPost]
        public async Task<ActionResult> AddPlace(Place place)
        {
            await _repository.AddPlaceAsync(place);
            return CreatedAtAction(nameof(GetPlaceById), new { id = place.Id }, place);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdatePlace(int id, Place place)
        {
            if (id != place.Id)
                return BadRequest();

            await _repository.UpdatePlaceAsync(place);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePlace(int id)
        {
            await _repository.DeletePlaceAsync(id);
            return NoContent();
        }

        [HttpGet("location/{location}")]
        public async Task<ActionResult<IEnumerable<Place>>> GetPlacesByLocation(string location)
        {
            var places = await _repository.GetPlacesByLocationAsync(location);
            return Ok(places);
        }
    }
}