using CareerCompassAPI.Application.Abstraction.Services;
using CareerCompassAPI.Application.DTOs.Vacancy_DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CareerCompassAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class VacanciesController : ControllerBase
    {
        private readonly IVacancyService _vacancyService;

        public VacanciesController(IVacancyService vacancyService)
        {
            _vacancyService = vacancyService;
        }
        [HttpPost]
        public async Task<IActionResult> Post(VacancyCreateDto vacancyCreateDto, [FromQuery] string userId, [FromQuery] Guid companyId)
        {
            await _vacancyService.Create(vacancyCreateDto, userId, companyId);
            return Ok();
        }
        [HttpGet("[action]")]
        [AllowAnonymous]
        public async Task<IActionResult> GetBySearch(string jobTitle)
        {
            var response = await _vacancyService.GetBySearch(jobTitle);
            return Ok(response);
        }
        [HttpGet("GetFilteredSearch")]
        [AllowAnonymous]
        public async Task<ActionResult<List<VacancyGetDetailsDto>>> GetFilteredSearch(
          [FromQuery(Name = "job-title")] string? jobTitle,
          [FromQuery(Name = "location-id")] Guid? locationId,
          [FromQuery] int page = 1,
          [FromQuery] int pageSize = 3
      )
        {
            var vacancies = await _vacancyService.GetDetailsBySearch(jobTitle, locationId, page, pageSize);
            return Ok(vacancies);
        }

        [HttpGet("[action]")]   
        [AllowAnonymous]
        public async Task<IActionResult> GetVacanciesById(Guid id)
        {
            var response = await _vacancyService.GetVacancyByRecruiterId(id);
            return Ok(response);    
        }
        [HttpDelete("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> DeleteVacancy(Guid id)
        {
           await _vacancyService.DeleteVacancyById(id);
           return NoContent();
        }
    }
}
