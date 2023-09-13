using CareerCompassAPI.Application.Abstraction.Services;
using CareerCompassAPI.Application.DTOs.Dashboard_DTOs;
using CareerCompassAPI.Domain.Enums;
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
        public async Task<IActionResult> GetAllUsers([FromQuery] string? searchQuery)
        {
            var response = await _dashboardService.GetAllAsync(searchQuery);
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
        [HttpGet("[action]")]
        public async Task<IActionResult> GetPendingReviews()
        {
            var response = await _dashboardService.GetAllPendingReviews();
            return Ok(response);
        }
        [HttpPatch("[action]")]
      
        public async Task<IActionResult> UpdateReviewStatus([FromQuery] Guid reviewId, [FromBody] int newStatus)
        {
            if (Enum.IsDefined(typeof(ReviewStatus), newStatus))
            {
                await _dashboardService.UpdateReviewStatus(reviewId, (ReviewStatus)newStatus);
                return Ok();
            }
            return BadRequest("Invalid Status");
        }
        [HttpGet("[action]")]
        [AllowAnonymous]
        public async Task<IActionResult> GetUserStats(DateTime? startDate, DateTime? endDate)
        {
            var stats = await _dashboardService.GetUserRegistrationStatsAsync(startDate, endDate);
            return Ok(stats);
        }
        [HttpGet("[action]")]
        public async Task<IActionResult> GetEducationLevels()
        {
            var response = await _dashboardService.GetAllEducationLevelsAsync();
            return Ok(response);
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> CreateEducationLevel(CreateEducationLevelDto createEducationLevelDto)
        {
            await _dashboardService.CreateEducationLevel(createEducationLevelDto);
            return Ok();
        }
        [HttpDelete("[action]")]
        public async Task<IActionResult> RemoveEducationLevel([FromQuery] Guid levelId)
        {
            await _dashboardService.RemoveEducationLevel(levelId);
            return Ok();
        }
        [HttpPatch("[action]")]
        public async Task<IActionResult> UpdateEducationLevel(EducationLevelUpdateDto educationLevelUpdateDto)
        {
            await _dashboardService.UpdateEducationLevel(educationLevelUpdateDto);
            return Ok();
        }
    }
}
