using Microsoft.AspNetCore.Mvc;
using TourismGalle.Models;
using TourismGalle.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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

        // Get all travel places
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TravelPlace>>> GetAllTravelPlaces()
        {
            try
            {
                var places = await _repository.GetAllTravelPlacesAsync();
                return Ok(places);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // Get a specific travel place by ID
        [HttpGet("{id}")]
        public async Task<ActionResult<TravelPlace>> GetTravelPlaceById(int id)
        {
            try
            {
                var place = await _repository.GetTravelPlaceByIdAsync(id);
                if (place == null)
                    return NotFound(new { message = "Travel place not found" });

                return Ok(place);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // Add a new travel place along with facilities
        [HttpPost]
        public async Task<ActionResult> AddTravelPlace(TravelPlace place)
        {
            try
            {
                // Ensure the facilities are attached to the travel place
                if (place.Facilities == null || place.Facilities.Count == 0)
                {
                    return BadRequest(new { message = "At least one facility must be provided" });
                }

                var id = await _repository.AddTravelPlaceAsync(place, place.Facilities);
                return CreatedAtAction(nameof(GetTravelPlaceById), new { id }, place);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // Update an existing travel place and its facilities
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateTravelPlace(int id, TravelPlace place)
        {
            if (id != place.Id)
                return BadRequest(new { message = "ID mismatch" });

            try
            {
                // Validate if facilities are present for update
                if (place.Facilities == null || place.Facilities.Count == 0)
                {
                    return BadRequest(new { message = "At least one facility must be provided for the update" });
                }

                // Update the travel place and its facilities
                await _repository.UpdateTravelPlaceAsync(place, place.Facilities);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // Delete a travel place
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTravelPlace(int id)
        {
            try
            {
                var place = await _repository.GetTravelPlaceByIdAsync(id);
                if (place == null)
                    return NotFound(new { message = "Travel place not found" });

                await _repository.DeleteTravelPlaceAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
