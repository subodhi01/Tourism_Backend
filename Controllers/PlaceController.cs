using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using TourismGalle.Models;
using TourismGalle.Services;

namespace TourismGalle.Controllers
{
    [Route("api/places")]
    [ApiController]
    public class PlaceController : ControllerBase
    {
        private readonly PlaceService _placeService;

        public PlaceController(PlaceService placeService)
        {
            _placeService = placeService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Place>>> GetAllPlaces()
        {
            var places = await _placeService.GetAllPlaces();
            return Ok(places);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Place>> GetPlaceById(int id)
        {
            var place = await _placeService.GetPlaceById(id);
            if (place == null) return NotFound();
            return Ok(place);
        }

        [HttpPost]
        public async Task<ActionResult> AddPlace([FromBody] Place place)
        {
            await _placeService.AddPlace(place);
            return CreatedAtAction(nameof(GetPlaceById), new { id = place.Id }, place);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdatePlace(int id, [FromBody] Place place)
        {
            if (id != place.Id) return BadRequest();
            await _placeService.UpdatePlace(place);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePlace(int id)
        {
            await _placeService.DeletePlace(id);
            return NoContent();
        }
    }
}
