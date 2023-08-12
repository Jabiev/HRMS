using CareerCompassAPI.Application.Abstraction.Services;
using CareerCompassAPI.Application.DTOs.Industry_DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CareerCompassAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IndustriesController : ControllerBase
    {
        private readonly IIndustryService _industryService;
        public IndustriesController(IIndustryService industryService)
        {
            _industryService = industryService;
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> Create(IndustryCreateDto industryCreateDto)
        {
            await _industryService.CreateAsync(industryCreateDto);
            return Ok();
        }
    }
}
