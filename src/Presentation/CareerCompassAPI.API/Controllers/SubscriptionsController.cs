using CareerCompassAPI.Application.Abstraction.Services;
using CareerCompassAPI.Application.DTOs.Subscription_DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CareerCompassAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubscriptionsController : ControllerBase
    {
        private readonly ISubscriptionService _subscriptionService;

        public SubscriptionsController(ISubscriptionService subscriptionService)
        {
            _subscriptionService = subscriptionService;
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> Post(SubscriptionCreateDto subscriptionCreateDto)
        {
            await _subscriptionService.CreateAsync(subscriptionCreateDto);
            return StatusCode((int)HttpStatusCode.Created);
        }
        [HttpGet("[action]")]
        public async Task<IActionResult> GetAll()
        {
            var response = await _subscriptionService.GetAllAsync();
            return Ok(response);
        }
    }
}
