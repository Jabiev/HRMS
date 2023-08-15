using CareerCompassAPI.Application.Abstraction.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CareerCompassAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationsController : ControllerBase
    {
        private readonly ILocationService _locationService;
        public LocationsController(ILocationService locationService)
        {
            _locationService = locationService;
        }
        [HttpGet("[action]")]
        public async Task<IActionResult> GetAll()
        {
            var response = await _locationService.GetAll();
            return Ok(response);
        }
    }
}
