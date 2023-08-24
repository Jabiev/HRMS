using CareerCompassAPI.Application.Abstraction.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CareerCompassAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobSeekersController : ControllerBase
    {
        private readonly IJobSeekerService _jobSeekerService;

        public JobSeekersController(IJobSeekerService jobSeekerService)
        {
            _jobSeekerService = jobSeekerService;
        }
        [HttpGet("[action]")]
        public async Task<IActionResult> GetByUserId(Guid userId)
        {
            var response = await _jobSeekerService.GetByUserId(userId);
            return Ok(response);
        }
    }
}
