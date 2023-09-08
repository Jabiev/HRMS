using CareerCompassAPI.Application.Abstraction.Services;
using CareerCompassAPI.Application.DTOs.Follower_DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CareerCompassAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FollowersController : ControllerBase
    {
        private readonly IFollowerService _followerService;

        public FollowersController(IFollowerService followerService)
        {
            _followerService = followerService;
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> Follow(FollowerCreateDto followerCreateDto)
        {
            await _followerService.CreateAsync(followerCreateDto);
            return Ok();
        }
        [HttpDelete("[action]")]
        public async Task<IActionResult> Unfollow(FollowerRemoveDto followerRemoveDto)
        {
            await _followerService.Remove(followerRemoveDto);
            return Ok();
        }
        [HttpGet("[action]")]
        public async Task<IActionResult> GetFollowedCompanies(string appUserId)
        {
            var response = await _followerService.GetFollowedCompanies(appUserId);
            return Ok(response);
        }
    }
}
