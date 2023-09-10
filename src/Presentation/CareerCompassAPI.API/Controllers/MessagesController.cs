using CareerCompassAPI.Application.Abstraction.Services;
using CareerCompassAPI.Application.DTOs.Message_DTOs;
using CareerCompassAPI.SignalR.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace CareerCompassAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly IMessageService _messageService;
        private readonly IHubContext<ChatHub> _hubContext;

        public MessagesController(IMessageService messageService, IHubContext<ChatHub> hubContext)
        {
            _messageService = messageService;
            _hubContext = hubContext;
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> Send(MessageCreateDto messageCreateDto)
        {
            await _messageService.CreateAsync(messageCreateDto);
            //await _hubContext.Clients.All.SendAsync("ReceiveMessage");
            return Ok(new { Message = "Message successfully sent." });
        }
        [HttpGet("[action]")]
        public async Task<IActionResult> GetMessages(string senderId, string receiverId)
        {
            var response = await _messageService.GetUnreadMessagesAsync(senderId, receiverId);
            return Ok(response);
        }
    }
}
