using CareerCompassAPI.Application.Abstraction.Services;
using CareerCompassAPI.Application.DTOs.Dashboard_DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CareerCompassAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Master,Admin")]
    public class DashboardsController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;

        public DashboardsController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }
        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllUsers()
        {
            var response = await _dashboardService.GetAllAsync();
            return Ok(response);
        }
        [HttpDelete("[action]")]
        public async Task<IActionResult> RemoveUser([FromQuery] string appUserId)
        {
            await _dashboardService.RemoveUser(appUserId);
            return Ok();
        }
        [HttpPut("[action]")]
        public async Task<IActionResult> ChangeUserRole(ChangeUserRoleDto changeUserRoleDto)
        {
            await _dashboardService.ChangeUserRole(changeUserRoleDto);
            return Ok(new { Message = "User role updated successfully" });
        }
        [HttpGet("[action]")]
     
        public async Task<IActionResult> GetAllCompanies([FromQuery] string? sortOrder, [FromQuery] string? searchQuery)
        {
            var response = await _dashboardService.GetAllCompaniesAsync(sortOrder, searchQuery);
            return Ok(response);
        }
        [HttpDelete("[action]")]
        public async Task<IActionResult> RemoveCompany([FromQuery] Guid companyId)
        {
            await _dashboardService.RemoveCompany(companyId);
            return Ok();
        }
    }
}
