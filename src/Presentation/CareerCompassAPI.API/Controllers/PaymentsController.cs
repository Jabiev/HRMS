using CareerCompassAPI.Application.Abstraction.Services;
using CareerCompassAPI.Application.DTOs.Payment_DTOs;
using CareerCompassAPI.Domain.Stripe;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace CareerCompassAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly IStripeAppService _stripeService;
        public PaymentsController(IStripeAppService stripeService)
        {
            _stripeService = stripeService;
        }
        [HttpPost("[action]")]
        public async Task<ActionResult<StripeCustomer>> AddStripeCustomer([FromBody] AddStripeCustomer customer, CancellationToken ct)
        {
            StripeCustomer createdCustomer = await _stripeService.AddStripeCustomerAsync(
               customer,
               ct);

            return StatusCode(StatusCodes.Status200OK, createdCustomer);
        }
        [HttpPost("[action]")]
        public async Task<ActionResult<StripePayment>> AddStripePayment([FromBody] AddStripePayment payment, CancellationToken ct)
        {
            StripePayment createdPayment = await _stripeService.AddStripePaymentAsync(payment, ct);
            return StatusCode(StatusCodes.Status200OK, createdPayment);  
        }
        [HttpPost("[action]")]
        public async Task<ActionResult<string>> CreateCheckoutSession([FromBody] PlanDTO plan, CancellationToken ct)
        {
            string sessionId = await _stripeService.CreateCheckoutSessionAsync(plan, ct);
            return Ok(new { sessionId = sessionId }); 
        }
    }
}
