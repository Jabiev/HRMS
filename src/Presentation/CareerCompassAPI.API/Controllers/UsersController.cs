using CareerCompassAPI.Application.Abstraction.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CareerCompassAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpGet("[action]")]
        public async Task<IActionResult> GetUserDetails(string userId)
        {
            var response = await _userService.GetDetailsAsync(userId);
            return Ok(response);
        }
    }
}
