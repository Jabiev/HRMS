using CareerCompassAPI.Application.Abstraction.Services;
using CareerCompassAPI.Application.DTOs.JobSeeker_DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CareerCompassAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class JobSeekersController : ControllerBase
    {
        private readonly IJobSeekerService _jobSeekerService;

        public JobSeekersController(IJobSeekerService jobSeekerService)
        {
            _jobSeekerService = jobSeekerService;
        }
        [HttpGet("[action]")]
        public async Task<IActionResult> GetByUserId([FromQuery]Guid userId)
        {
            var response = await _jobSeekerService.GetByUserId(userId);
            return Ok(response);
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> Post(JobSeekerCreateDto createDto ,[FromQuery]string userId)
        {
            await _jobSeekerService.CreateAsync(createDto, userId);
            return Ok();
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> UploadAvatar([FromQuery] string jobseekerId, JobseekerAvatarUploadDto uploadAvatar)
        {
            await _jobSeekerService.UploadLogoAsync(jobseekerId,uploadAvatar);
            return Ok();
        }
    }
}
