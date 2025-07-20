using CareerCompassAPI.Application.Abstraction.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CareerCompassAPI.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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

    [HttpPost("[action]/{notificationId}")]
    public async Task<IActionResult> MarkNotificationAsRead(Guid notificationId)
    {
        await _notificationService.MarkAsReadAsync(notificationId);
        return Ok();
    }
}
