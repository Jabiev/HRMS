using CareerCompassAPI.Application.Abstraction.Services;
using CareerCompassAPI.Application.DTOs.Feedback_DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CareerCompassAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedbacksController : ControllerBase
    {
        private readonly IFeedbackService _feedbackService;

        public FeedbacksController(IFeedbackService feedbackService)
        {
            _feedbackService = feedbackService;
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> Post(FeedbackCreateDto feedbackCreateDto)
        {
            await _feedbackService.CreateFeedbackAsync(feedbackCreateDto);
            return Ok();
        }
        [HttpGet("[action]")]
        public async Task<IActionResult> GetTestimonials()
        {
            var response = await _feedbackService.GetFeedbacksAsync();
            return Ok(response);
        }
    }
}
