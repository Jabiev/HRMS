using CareerCompassAPI.Application.Abstraction.Repositories.IJobSeekerRepositories;
using CareerCompassAPI.Application.Abstraction.Repositories.IRecruiterRepositories;
using CareerCompassAPI.Application.Abstraction.Services;
using CareerCompassAPI.Application.DTOs.Payment_DTOs;
using CareerCompassAPI.Domain.Enums;
using CareerCompassAPI.Domain.Stripe;
using CareerCompassAPI.SignalR.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Stripe;
using Stripe.Checkout;

namespace CareerCompassAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly IStripeAppService _stripeService;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<PaymentsController> _logger;
        private readonly IPaymentsService _paymentsService;
        private readonly IRecruiterReadRepository _recruiterReadRepository;
        private readonly IJobSeekerReadRepository _jobSeekerReadRepository;
        private const string WebhookSecret = "whsec_587bf44d90eabd10e52b11efb70476cd2e945a394a3d5cd005a236097741f3bd";
        public PaymentsController(IStripeAppService stripeService,
                                  IPaymentsService paymentsService,
                                  IRecruiterReadRepository recruiterReadRepository,
                                  IJobSeekerReadRepository jobSeekerReadRepository,
                                  IServiceProvider serviceProvider,
                                  ILogger<PaymentsController> logger)
        {
            _stripeService = stripeService;
            _paymentsService = paymentsService;
            _recruiterReadRepository = recruiterReadRepository;
            _jobSeekerReadRepository = jobSeekerReadRepository;
            _serviceProvider = serviceProvider;
            _logger = logger;
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

                        if (session.Metadata.ContainsKey("recruiter_id"))
                        {
                            var planName = session.Metadata["plan_name"];
                            var recruiterId = Guid.Parse(session.Metadata["recruiter_id"]);
                            var recruiter = await _recruiterReadRepository.GetByIdAsync(recruiterId);

                            if (string.IsNullOrEmpty(planName))
                            {
                                // Handle errors related to missing plan name
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
                        else if (session.Metadata.ContainsKey("job_seeker_id"))
                        {
                            var jobSeekerId = Guid.Parse(session.Metadata["job_seeker_id"]);
                            long amountTotal = session.AmountTotal ?? throw new InvalidOperationException("Amount total is missing");
                            var jobSeeker = Guid.Parse(session.Metadata["job_seeker_id"]);
                            var js = await _jobSeekerReadRepository.GetByUserIdAsync(jobSeekerId);
                            var paymentCreateDto = new PaymentCreateDto(
                               jobSeeker.ToString(),
                                amountTotal,
                                PaymentTypes.Resume
                                );
                            await _paymentsService.CreateAsync(paymentCreateDto);

                            _logger.LogInformation($"Sending PaymentSuccess message to user {jobSeekerId}.");
                            var hubContext = _serviceProvider.GetService<IHubContext<PaymentHub>>();
                            await hubContext.Clients.User(jobSeekerId.ToString()).SendAsync("PaymentSuccess");

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


        [HttpPost("[action]")]
        public async Task<ActionResult<string>> CreateResumeCheckoutSession([FromBody] JobSeekerResumeCreateDto jobSeekerResume, CancellationToken ct)
        {
            string sessionId = await _stripeService.CreateCheckoutSessionForResumeAsync(jobSeekerResume, ct);
            return Ok(new { sessionId = sessionId });
        }
        [HttpGet("GetPayments/{userId}")]
        public async Task<IActionResult> GetPayments([FromRoute]string userId)
        {
            var response = await _paymentsService.GetPaymentsByAppUserId(userId);
            return Ok(response);
        }
    }
}
