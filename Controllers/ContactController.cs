using Microsoft.AspNetCore.Mvc;
using TourismGalle.Models;
using TourismGalle.Services;

namespace TourismGalle.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        private readonly ContactRepository _repository;

        public ContactController(ContactRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Contact>>> GetAllContacts()
        {
            var contacts = await _repository.GetAllContactsAsync();
            return Ok(contacts);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Contact>> GetContactById(int id)
        {
            var contact = await _repository.GetContactByIdAsync(id);
            if (contact == null)
                return NotFound();
            return Ok(contact);
        }

        [HttpPost]
        public async Task<ActionResult> AddContact(Contact contact)
        {
            await _repository.AddContactAsync(contact);
            return CreatedAtAction(nameof(GetContactById), new { id = contact.ContactID }, contact);
        }

        [HttpPut("{id}/status")]
        public async Task<ActionResult> UpdateContactStatus(int id, [FromBody] bool isRead)
        {
            await _repository.UpdateContactStatusAsync(id, isRead);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteContact(int id)
        {
            await _repository.DeleteContactAsync(id);
            return NoContent();
        }
    }
}