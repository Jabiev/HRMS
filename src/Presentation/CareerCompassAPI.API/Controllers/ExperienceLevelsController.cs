using CareerCompassAPI.Application.Abstraction.Services;
using Microsoft.AspNetCore.Mvc;

namespace CareerCompassAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExperienceLevelsController : ControllerBase
    {
        private readonly IExperienceLevelService _experienceLevelService;

        public ExperienceLevelsController(IExperienceLevelService experienceLevelService)
        {
            _experienceLevelService = experienceLevelService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await _experienceLevelService.GetAllAsync();
            return Ok(response);
        }
    }
}
