using CareerCompassAPI.Application.Abstraction.Services;
using CareerCompassAPI.Application.DTOs.Event_DTOs;
using Microsoft.AspNetCore.Mvc;

namespace CareerCompassAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly IEventService _eventService;

        public EventsController(IEventService eventService)
        {
            _eventService = eventService;
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> CreateEvent(EventCreateDto eventCreateDto)
        {
            var eventId = await _eventService.Create(eventCreateDto);
            return Ok(eventId);
        }
        [HttpGet("[action]")]
        public async Task<IActionResult> GetEvents([FromQuery] string userId)
        {
            var response = await _eventService.GetEventsByUserId(userId);
            return Ok(response);
        }
        [HttpPut("[action]")]
        public async Task<IActionResult> UpdateEvent(EventUpdateDto eventUpdateDto)
        {
            await _eventService.UpdateEvent(eventUpdateDto);
            return Ok();
        }
    }
}
