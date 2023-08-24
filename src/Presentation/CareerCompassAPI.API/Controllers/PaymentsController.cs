using CareerCompassAPI.Application.Abstraction.Repositories.IRecruiterRepositories;
using CareerCompassAPI.Application.Abstraction.Services;
using CareerCompassAPI.Application.DTOs.Payment_DTOs;
using CareerCompassAPI.Domain.Enums;
using CareerCompassAPI.Domain.Identity;
using CareerCompassAPI.Domain.Stripe;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.Checkout;

namespace CareerCompassAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly IStripeAppService _stripeService;
        private readonly ISubscriptionService _subscriptionService;
        private readonly IPaymentsService _paymentsService;
        private readonly IRecruiterReadRepository _recruiterReadRepository;
        private const string WebhookSecret = "whsec_587bf44d90eabd10e52b11efb70476cd2e945a394a3d5cd005a236097741f3bd";
        public PaymentsController(IStripeAppService stripeService,
                                  ISubscriptionService subscriptionService,
                                  IPaymentsService paymentsService,
                                  IRecruiterReadRepository recruiterReadRepository)
        {
            _stripeService = stripeService;
            _subscriptionService = subscriptionService;
            _paymentsService = paymentsService;
            _recruiterReadRepository = recruiterReadRepository;
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
        [HttpPost("[action]")]
        public async Task<IActionResult> HandleWebhook(CancellationToken ct)
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

            try
            {
                var stripeEvent = EventUtility.ConstructEvent(
                    json,
                    Request.Headers["Stripe-Signature"],
                    WebhookSecret
                );

                if (stripeEvent.Type == Events.CheckoutSessionCompleted)
                {

                    var session = stripeEvent.Data.Object as Session;
                    if (session != null)
                    {
                        var sessionId = session.Id;
                        var planName = session.Metadata["plan_name"];
                        var recruiterId = Guid.Parse(session.Metadata["recruiter_id"]);
                        var recruiter = await _recruiterReadRepository.GetByIdAsync(recruiterId);
                        if (string.IsNullOrEmpty(planName))
                        {
                            // errors
                        }
                        else
                        {
                            long amountTotal = session.AmountTotal ?? throw new InvalidOperationException("Amount total is missing");
                            await _stripeService.UpdateSubscriptionAsync(sessionId, planName);
                            var paymentCreateDto = new PaymentCreateDto(
                            recruiter.AppUserId,
                            amountTotal,
                            PaymentTypes.Subscription 
                            );

                            await _paymentsService.CreateAsync(paymentCreateDto);
                        }
                    }
                }

                return Ok();
            }
            catch (StripeException e)
            {
                Console.WriteLine($"StripeException: {e.Message}");
                return BadRequest();
            }
        }

    }
}
