using CareerCompassAPI.Application.Abstraction.Services;
using CareerCompassAPI.Application.DTOs.Industry_DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CareerCompassAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Master,Admin")]
    public class IndustriesController : ControllerBase
    {
        private readonly IIndustryService _industryService;
        public IndustriesController(IIndustryService industryService)
        {
            _industryService = industryService;
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> Create([FromBody] IndustryCreateDto industryCreateDto)
        {
            await _industryService.CreateAsync(industryCreateDto);
            return StatusCode((int)HttpStatusCode.Created);
        }

        [HttpGet("[action]")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            List<IndustryGetDto> result = await _industryService.GetAllAsync();
            return Ok(result);
        }
        [HttpDelete("[action]/{industryId}")]
        [AllowAnonymous]
        public async Task<IActionResult> Remove([FromRoute]Guid industryId)
        {
            await _industryService.Remove(industryId);
            return Ok();
        }
    }
}
