using CareerCompassAPI.Application.Abstraction.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CareerCompassAPI.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class ExperienceLevelsController : ControllerBase
{
    private readonly IExperienceLevelService _experienceLevelService;

    public ExperienceLevelsController(IExperienceLevelService experienceLevelService)
    {
        _experienceLevelService = experienceLevelService;
    }

    [HttpGet("[action]")]
    public async Task<IActionResult> GetAll()
    {
        var response = await _experienceLevelService.GetAllAsync();
        return Ok(response);
    }
}
