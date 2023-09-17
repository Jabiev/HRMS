using CareerCompassAPI.Application.Abstraction.Services;
using Microsoft.AspNetCore.Mvc;

namespace CareerCompassAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EducationLevelsController : ControllerBase
    {
        private readonly IEducationLevelService _educationLevelService;

        public EducationLevelsController(IEducationLevelService educationLevelService)
        {
            _educationLevelService = educationLevelService;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetAll()
        {
            var response = await _educationLevelService.GetAllAsync();
            return Ok(response);
        }
    }
}
