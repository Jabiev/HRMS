using CareerCompassAPI.Application.Abstraction.Services;
using CareerCompassAPI.Application.DTOs.Dashboard_DTOs;
using CareerCompassAPI.Application.DTOs.Subscription_DTOs;
using CareerCompassAPI.Application.DTOs.TeamMember_DTOs;
using CareerCompassAPI.Domain.Enums;
using CareerCompassAPI.Persistence.Exceptions;
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
        public async Task<IActionResult> GetAllUsers([FromQuery] string? searchQuery, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var response = await _dashboardService.GetAllAsync(searchQuery, pageNumber, pageSize);
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
            var newLevelId = await _dashboardService.CreateEducationLevel(createEducationLevelDto);
            return Ok(newLevelId);
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

        [HttpGet("[action]")]
        public async Task<IActionResult> GetExperienceLevels()
        {
            var response = await _dashboardService.GetAllExperienceLevelsAsync();
            return Ok(response);
        }
        [HttpDelete("[action]")]
        public async Task<IActionResult> RemoveExperienceLevel([FromQuery] Guid levelId)
        {
            await _dashboardService.RemoveExperienceLevel(levelId);
            return Ok();
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> CreateExperienceLevel(ExperienceLevelCreateDto experienceLevelCreateDto)
        {
            var newLevelId = await _dashboardService.CreateExperienceLevel(experienceLevelCreateDto);
            return Ok(newLevelId);
        }
        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllJobLocations([FromQuery] string? searchQuery)
        {
            var response = await _dashboardService.GetAllLocationsAsync(searchQuery);
            return Ok(response);
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> CreateJobLocation(JobLocationCreateDto jobLocationCreateDto)
        {
            var locationId = await _dashboardService.CreateJobLocation(jobLocationCreateDto);
            return Ok(locationId);
        }
        [HttpDelete("[action]")]
        public async Task<IActionResult> RemoveJobLocation([FromQuery] Guid joblocationId)
        {
            await _dashboardService.RemoveJobLocation(joblocationId);
            return Ok();
        }
        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllJobTypes()
        {
            var response = await _dashboardService.GetAllTypesAsync();
            return Ok(response);
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> CreateJobType(JobTypeCreateDto jobTypeCreateDto)
        {
            await _dashboardService.CreateJobType(jobTypeCreateDto);
            return Ok();
        }
        [HttpDelete("[action]")]
        public async Task<IActionResult> RemoveJobType([FromQuery] Guid jobTypeId)
        {
            await _dashboardService.RemoveJobType(jobTypeId);
            return Ok();
        }
        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllSubscriptions()
        {
            var response = await _dashboardService.GetAllSubscriptionsAsync();
            return Ok(response);
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> CreateSubscription(SubscriptionCreateDto subscriptionCreateDto)
        {
            var response = await _dashboardService.CreateSubscription(subscriptionCreateDto);
            return Ok(response);
        }
        [HttpDelete("[action]")]
        public async Task<IActionResult> RemoveSubscription([FromQuery] Guid subscriptionId)
        {
            await _dashboardService.RemoveSubscription(subscriptionId);
            return Ok();
        }
        [HttpPut("[action]")]
        public async Task<IActionResult> UpdateSubscription(SubscriptionUpdateDto subscriptionUpdateDto)
        {
            await _dashboardService.UpdateSubscription(subscriptionUpdateDto);
            return Ok();
        }
        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllShifts()
        {
            var response = await _dashboardService.GetAllShiftsAsync();
            return Ok(response);
        }
        [HttpDelete("[action]")]
        public async Task<IActionResult> RemoveShift([FromQuery] Guid shiftId)
        {
            await _dashboardService.RemoveShift(shiftId);
            return Ok();
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> CreateShift(ShiftAndScheduleCreateDto shiftAndScheduleDto)
        {
            var response = await _dashboardService.CreateShift(shiftAndScheduleDto);
            return Ok(response);
        }
        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllPayments(int page = 1, int pageSize = 10)
        {
            try
            {
                var response = await _dashboardService.GetAllPaymentsAsync(page, pageSize);
                return Ok(response);
            }
            catch (NotFoundException)
            {
                return NotFound("No Payments found");
            }
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllUserFeedbacks(int page = 1, int pageSize = 10)
        {
            var response = await _dashboardService.GetAllFeedbacksAsync(page, pageSize);
            return Ok(response);
        }
        [HttpDelete("[action]")]
        public async Task<IActionResult> RemoveFeedback([FromQuery] Guid feedbackId)
        {
            await _dashboardService.RemoveFeedback(feedbackId);
            return Ok();
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> SetIsActive([FromQuery] Guid feedbackId)
        {
            await _dashboardService.SetIsActive(feedbackId);
            return Ok();
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> CreateTeamMember(TeamMemberCreateDto teamMemberCreateDto)
        {
            var responseId = await _dashboardService.CreateMember(teamMemberCreateDto);
            return Ok(responseId);
        }
        [HttpGet("[action]")]
        public async Task<IActionResult> GetMembers()
        {
            var response = await _dashboardService.GetAllTeamMembers();
            return Ok(response);
        }
        [HttpGet("[action]")]
        public async Task<IActionResult> GetSettings()
        {
            var response = await _dashboardService.GetAppSettingListAsync();
            return Ok(response);
        }
        [HttpGet("[action]")]
        public async Task<IActionResult> GetResumes()
        {
            var response = await _dashboardService.GetAllResumes();
            return Ok(response);    
        }
    }
}
