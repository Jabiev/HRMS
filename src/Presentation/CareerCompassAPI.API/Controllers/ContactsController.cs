using CareerCompassAPI.Application.Abstraction.Services;
using CareerCompassAPI.Application.DTOs.Contact_DTOs;
using Microsoft.AspNetCore.Mvc;

namespace CareerCompassAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactsController : ControllerBase
    {
        private readonly IContactService _contactService;

        public ContactsController(IContactService contactService)
        {
            _contactService = contactService;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> CreateContact(ContactCreateDto contactCreateDto)
        {
            await _contactService.SendContactEmail(contactCreateDto);
            return Ok(new { Message = "Contact email sent successfully" });
        }
    }
}
