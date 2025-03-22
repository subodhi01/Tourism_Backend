using Microsoft.AspNetCore.Mvc;
using TourismGalle.Models;
using TourismGalle.Services;

namespace TourismGalle.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TourPackageController : ControllerBase
    {
        private readonly TourPackageRepository _repository;

        public TourPackageController(TourPackageRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TourPackage>>> GetAllTourPackages()
        {
            var packages = await _repository.GetAllTourPackagesAsync();
            return Ok(packages);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TourPackage>> GetTourPackageById(int id)
        {
            var package = await _repository.GetTourPackageByIdAsync(id);
            if (package == null)
                return NotFound();
            return Ok(package);
        }

        [HttpPost]
        public async Task<ActionResult> AddTourPackage(TourPackage package)
        {
            await _repository.AddTourPackageAsync(package);
            return CreatedAtAction(nameof(GetTourPackageById), new { id = package.PackageID }, package);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateTourPackage(int id, TourPackage package)
        {
            if (id != package.PackageID)
                return BadRequest();

            await _repository.UpdateTourPackageAsync(package);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTourPackage(int id)
        {
            await _repository.DeleteTourPackageAsync(id);
            return NoContent();
        }

        [HttpGet("place/{place}")]
        public async Task<ActionResult<IEnumerable<TourPackage>>> GetTourPackagesByPlace(string place)
        {
            var packages = await _repository.GetTourPackagesByPlaceAsync(place);
            return Ok(packages);
        }

    }
}
