using CareerCompassAPI.Application.Abstraction.Services;
using CareerCompassAPI.Application.DTOs.Vacancy_DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CareerCompassAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
    }
}
