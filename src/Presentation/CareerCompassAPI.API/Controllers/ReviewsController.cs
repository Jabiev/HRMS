using CareerCompassAPI.Application.Abstraction.Services;
using CareerCompassAPI.Application.DTOs.Review_DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CareerCompassAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private readonly IReviewService _reviewService;
        public ReviewsController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> PostReview(ReviewCreateDto reviewCreateDto)
        {
            await _reviewService.CreateAsync(reviewCreateDto);
            return Ok();
        }
    }
}
