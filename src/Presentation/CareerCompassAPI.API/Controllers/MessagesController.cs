using CareerCompassAPI.Application.Abstraction.Services;
using CareerCompassAPI.Application.DTOs.Message_DTOs;
using Microsoft.AspNetCore.Mvc;

namespace CareerCompassAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly IMessageService _messageService;

        public MessagesController(IMessageService messageService)
        {
            _messageService = messageService;
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> Send(MessageCreateDto messageCreateDto)
        {
            await _messageService.CreateAsync(messageCreateDto);
            return Ok(new { Message = "Message successfully sent." });
        }
    }
}
