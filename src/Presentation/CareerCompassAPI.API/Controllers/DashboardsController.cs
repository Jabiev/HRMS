using CareerCompassAPI.Application.Abstraction.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CareerCompassAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Master,Admin")]
    public class DashboardsController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;

        public DashboardsController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }
        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllUsers()
        {
            var response = await _dashboardService.GetAllAsync();
            return Ok(response);
        }
        [HttpDelete("[action]")]
        public async Task<IActionResult> RemoveUser([FromQuery] string appUserId)
        {
            await _dashboardService.RemoveUser(appUserId);
            return Ok();
        }
    }
}
