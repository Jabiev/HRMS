using CareerCompassAPI.Application.Abstraction.Services;
using CareerCompassAPI.Application.DTOs.Application_DTOs;
using CareerCompassAPI.Domain.Enums;
using CareerCompassAPI.SignalR.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace CareerCompassAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobApplicationsController : ControllerBase
    {
        private readonly IApplicationService _applicationService;
        private readonly IHubContext<ApplicationHub> _hubContext;

        public JobApplicationsController(IApplicationService applicationService,
                                         IHubContext<ApplicationHub> hubContext)
        {
            _applicationService = applicationService;
            _hubContext = hubContext;
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> Post(ApplicationCreateDto applicationCreateDto)
        {
            int newCurrentApplicationCount = await _applicationService.CreateAsync(applicationCreateDto);
            await _hubContext.Clients.All.SendAsync("ReceiveApplicationUpdate", newCurrentApplicationCount);
            return Ok();
        }
        [HttpGet("[action]")]
        public async Task<IActionResult> GetApplicants([FromQuery]string appUserId)
        {
            var response = await _applicationService.GetApplicationsByAppUserId(appUserId);
            return Ok(response);
        }
        [HttpGet("[action]/{appUserId}")]
        public async Task<IActionResult> GetApprovedApplicants([FromRoute] string appUserId)
        {
            var response = await _applicationService.GetApprovedApplicantsByAppUserId(appUserId);
            return Ok(response);
        }
        [HttpPut("[action]")]
        public async Task<IActionResult> UpdateStatus([FromBody] ApplicationStatusUpdateDto applicationStatusUpdateDto )
        {
            await _applicationService.UpdateAsync(applicationStatusUpdateDto);
            return Ok();
        }
    }
}
