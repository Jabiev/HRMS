using CareerCompassAPI.Application.Abstraction.Services;
using CareerCompassAPI.Application.DTOs.Resume_DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CareerCompassAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class ResumesController : ControllerBase
    {
        private readonly IResumeService _resumeService;

        public ResumesController(IResumeService resumeService)
        {
            _resumeService = resumeService;
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> CreateResume(ResumeCreateDto resumeCreateDto)
        {
            var resumeId = await _resumeService.CreateResume(resumeCreateDto);
            return Ok(resumeId);
        }
        [HttpGet("[action]")]
        public async Task<IActionResult> GetResumes()
        {
            var response = await _resumeService.GetAllResumes();
            return Ok(response);
        }
        [HttpGet("GetResumeById")]
        public async Task<IActionResult> GetResumeById([FromQuery] Guid id)
        {
            var resume = await _resumeService.GetResumeById(id);
            return Ok(resume);
        }
    }
}
