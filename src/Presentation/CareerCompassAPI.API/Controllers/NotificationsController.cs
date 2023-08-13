using CareerCompassAPI.Application.Abstraction.Services;
using Microsoft.AspNetCore.Mvc;

namespace CareerCompassAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly INotificationService _notificationService;
        public NotificationsController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }
        [HttpGet("[action]/{userId}")]
        public async Task<IActionResult> GetNotifications(Guid userId)
        {
            var response = await _notificationService.GetNotificationsAsync(userId);
            return Ok(response);
        }
    }
}
