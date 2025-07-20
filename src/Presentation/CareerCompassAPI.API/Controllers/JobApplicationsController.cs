using CareerCompassAPI.Application.Abstraction.Services;
using CareerCompassAPI.Application.DTOs.Application_DTOs;
using CareerCompassAPI.SignalR.Hubs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace CareerCompassAPI.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class JobApplicationsController : ControllerBase
{
    private readonly IApplicationService _applicationService;
    private readonly IHubContext<ApplicationHub> _hubContext;
    private readonly IJobSeekerService _jobSeekerService;
    //TODO: LOOK AT THEN FOR AUTH
    public JobApplicationsController(IApplicationService applicationService,
                                     IHubContext<ApplicationHub> hubContext,
                                     IJobSeekerService jobSeekerService)
    {
        _applicationService = applicationService;
        _hubContext = hubContext;
        _jobSeekerService = jobSeekerService;
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> Post(ApplicationCreateDto applicationCreateDto)
    {
        int newCurrentApplicationCount = await _applicationService.CreateAsync(applicationCreateDto);
        await _hubContext.Clients.All.SendAsync("ReceiveApplicationUpdate", newCurrentApplicationCount);
        return Ok();
    }

    [HttpGet("[action]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "RequireMasterAdminRecruiter")]
    public async Task<IActionResult> GetApplicants([FromQuery] string appUserId)
    {
        var response = await _applicationService.GetApplicationsByAppUserId(appUserId);
        return Ok(response);
    }

    [HttpGet("[action]/{appUserId}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "RequireMasterAdminRecruiter")]
    public async Task<IActionResult> GetApprovedApplicants([FromRoute] string appUserId)
    {
        var response = await _applicationService.GetApprovedApplicantsByAppUserId(appUserId);
        return Ok(response);
    }

    [HttpPut("[action]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "RequireMasterAdminRecruiter")]
    public async Task<IActionResult> UpdateStatus([FromBody] ApplicationStatusUpdateDto applicationStatusUpdateDto)
    {
        await _applicationService.UpdateAsync(applicationStatusUpdateDto);
        return Ok();
    }

    [HttpGet("[action]/{appUserId}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "RequireMasterAdminRecruiter")]
    public async Task<IActionResult> GetApprovedPositions([FromRoute] string appUserId)
    {
        var response = await _jobSeekerService.GetApprovedPositionsByAppUserId(appUserId);
        return Ok(response);
    }

    [HttpDelete("[action]")]
    public async Task<IActionResult> RemoveApplication(Guid applicationId)
    {
        await _applicationService.Remove(applicationId);
        return Ok();
    }
}
