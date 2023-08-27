using CareerCompassAPI.Application.Abstraction.Services;
using CareerCompassAPI.Application.DTOs.Application_DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CareerCompassAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobApplicationsController : ControllerBase
    {
        private readonly IApplicationService _applicationService;

        public JobApplicationsController(IApplicationService applicationService)
        {
            _applicationService = applicationService;
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> Post(ApplicationCreateDto applicationCreateDto)
        {
            await _applicationService.CreateAsync(applicationCreateDto);
            return Ok();
        }
    }
}
