using CareerCompassAPI.Application.Abstraction.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CareerCompassAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecruitersController : ControllerBase
    {
        private readonly IRecruiterService _recruiterService;
        public RecruitersController(IRecruiterService recruiterService)
        {
            _recruiterService = recruiterService;
        }
        [HttpGet("[action]/{userId}")]
        public async Task<IActionResult> GetRecruiter(Guid userId)
        {
            var recruiter = await _recruiterService.GetRecruiterByUserId(userId);
            return Ok(recruiter);   
        }
    }
}
