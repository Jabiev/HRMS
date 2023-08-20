using CareerCompassAPI.Application.Abstraction.Services;
using CareerCompassAPI.Application.DTOs.Vacancy_DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
    }
}
