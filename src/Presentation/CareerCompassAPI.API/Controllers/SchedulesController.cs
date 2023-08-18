using CareerCompassAPI.Application.Abstraction.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CareerCompassAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SchedulesController : ControllerBase
    {
        private readonly IShiftScheduleService _shiftScheduleService;

        public SchedulesController(IShiftScheduleService shiftScheduleService)
        {
            _shiftScheduleService = shiftScheduleService;
        }
        [HttpGet("[action]")]
        public async Task<IActionResult> GetAll()
        {
            var response = await _shiftScheduleService.GetAllAsync();
            return Ok(response);    
        }
    }
}
