using CareerCompassAPI.Application.Abstraction.Services;
using CareerCompassAPI.Application.DTOs.Company_DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CareerCompassAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CompaniesController : ControllerBase
    {
        private readonly ICompanyService _companyService;
        private readonly ILogger<CompaniesController> _logger;
        public CompaniesController(ICompanyService companyService, ILogger<CompaniesController> logger)
        {
            _companyService = companyService;
            _logger = logger;
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> Create([FromBody] CompanyCreateDto companyCreateDto, [FromQuery] string userId)
        {
            await _companyService.CreateAsync(companyCreateDto, userId);
            return StatusCode((int)HttpStatusCode.Created);
        }


        [HttpDelete("[action]/{companyId}")]
        public async Task<IActionResult> Remove(Guid companyId)
        {
            await _companyService.Remove(companyId);
            return Ok();
        }
        [HttpGet("[action]")]
        public async Task<IActionResult> GetDetails([FromQuery] Guid companyId) 
        {
            CompanyGetDto response = await _companyService.GetCompanyDetailsById(companyId);
            return Ok(response);
        }
        [HttpGet("[action]")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCompanyDetails([FromQuery] string companyName)
        {
            var response = await _companyService.GetCompanyBySearchAsync(companyName);
            return Ok(response);    
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> UploadLogo([FromQuery] Guid companyId, CompanyLogoUploadDto companyLogoUploadDto)
        {
            await _companyService.UploadLogoAsync(companyId, companyLogoUploadDto);
            return Ok();
        }
        [HttpGet("[action]")]
        [AllowAnonymous]
        public async Task<IActionResult> GetHighestRated()
        {
            var response = await _companyService.GetHighestRated();
            return Ok(response);
        }
        [HttpPut("[action]")]

        public async Task<IActionResult> UpdateDetails(CompanyDetailsUpdateDto companyDetailsUpdateDto)
        {
            await _companyService.CompanyDetailsUpdate(companyDetailsUpdateDto);
            return Ok();
        }
    }
}
