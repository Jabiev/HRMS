using CareerCompassAPI.Application.Abstraction.Repositories.IRecruiterRepositories;
using CareerCompassAPI.Application.Abstraction.Repositories.ISubscriptionRepository;
using CareerCompassAPI.Application.Abstraction.Services;
using CareerCompassAPI.Application.DTOs.Payment_DTOs;
using CareerCompassAPI.Domain.Stripe;
using Hangfire;
using Stripe;
using Stripe.Checkout;

namespace CareerCompassAPI.Persistence.Implementations.Services
{
    public class StripeAppService : IStripeAppService
    {
        private readonly ChargeService _chargeService;
        private readonly CustomerService _customerService;
        private readonly TokenService _tokenService;
        private readonly SessionService _sessionService;
        private readonly ISubscriptionReadRepository _subscriptionReadRepository;
        private readonly IRecruiterReadRepository _recruiterReadRepository;
        private readonly IRecruiterWriteRepository _recruiterWriteRepository;
        public StripeAppService(
            ChargeService chargeService,
            CustomerService customerService,
            TokenService tokenService,
            SessionService sessionService,
            ISubscriptionReadRepository subscriptionReadRepository,
            ISubscriptionWriteRepository subscriptionWriteRepository,
            IRecruiterWriteRepository recruiterWriteRepository,
            IRecruiterReadRepository recruiterReadRepository)
        {
            _chargeService = chargeService;
            _customerService = customerService;
            _tokenService = tokenService;
            _sessionService = sessionService;
            _subscriptionReadRepository = subscriptionReadRepository;
            _subscriptionWriteRepository = subscriptionWriteRepository;
            _recruiterWriteRepository = recruiterWriteRepository;
            _recruiterReadRepository = recruiterReadRepository;
        }
        public async Task<StripeCustomer> AddStripeCustomerAsync(AddStripeCustomer customer, CancellationToken ct)
        {
            TokenCreateOptions tokenOptions = new()
            {
                Card = new TokenCardOptions
                {
                    Name = customer.Name,
                    Number = customer.CreditCard.CardNumber,
                    ExpYear = customer.CreditCard.ExpirationYear,
                    ExpMonth = customer.CreditCard.ExpirationMonth,
                    Cvc = customer.CreditCard.Cvc
                }
            };
            Token stripeToken = await _tokenService.CreateAsync(tokenOptions, null, ct);
            CustomerCreateOptions customerOptions = new CustomerCreateOptions
            {
                Name = customer.Name,
                Email = customer.Email,
                Source = stripeToken.Id
            };
            Customer createdCustomer = await _customerService.CreateAsync(customerOptions, null, ct);
            return new StripeCustomer(createdCustomer.Name, createdCustomer.Email, createdCustomer.Id);
        }

        public async Task<StripePayment> AddStripePaymentAsync(AddStripePayment payment, CancellationToken ct)
        {
            ChargeCreateOptions paymentOptions = new ChargeCreateOptions
            {
                Customer = payment.CustomerId,
                ReceiptEmail = payment.ReceiptEmail,
                Description = payment.Description,
                Currency = payment.Currency,
                Amount = payment.Amount
            };

            var createdPayment = await _chargeService.CreateAsync(paymentOptions, null, ct);

            return new StripePayment(
              createdPayment.CustomerId,
              createdPayment.ReceiptEmail,
              createdPayment.Description,
              createdPayment.Currency,
              createdPayment.Amount,
              createdPayment.Id);
        }
        public async Task<string> CreateCheckoutSessionAsync(PlanDTO plan, CancellationToken ct)
        {
            if (plan == null || plan.Amount <= 0)
            {
                throw new ArgumentException("Plan amount must be greater than 0", nameof(plan));
            }

            long unitAmount = (long)(plan.Amount * 100m);

            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<SessionLineItemOptions>
        {
            new SessionLineItemOptions
            {
                PriceData = new SessionLineItemPriceDataOptions
                {
                    UnitAmount = unitAmount,
                    Currency = "usd",
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = plan.Name
                    },
                },
                Quantity = 1,
            },
        },
                Mode = "payment",
                SuccessUrl = "http://localhost:3000/paymentsuccess",
                CancelUrl = "http://localhost:3000/paymenterror",
                Metadata = new Dictionary<string, string>
        {
            { "recruiter_id", plan.RecruiterId.ToString() },
            { "plan_name", plan.Name } 
        },
            };

            var session = await _sessionService.CreateAsync(options, cancellationToken: ct);

            return session.Id;
        }

        public async Task<Session> RetrieveSessionAsync(string sessionId)
        {
            var sessionService = new SessionService();
            var session = await sessionService.GetAsync(sessionId);
            var recruiterId = Guid.Parse(session.Metadata["recruiter_id"]);

            return session;
        }
        public async Task UpdateSubscriptionAsync(string sessionId, string planName)
        {
            var session = await RetrieveSessionAsync(sessionId);
            var recruiterId = Guid.Parse(session.Metadata["recruiter_id"]);

            if (string.IsNullOrEmpty(planName))
            {
                throw new ArgumentException("Plan name must not be empty.", nameof(planName));
            }

            var subscriptionId = await _recruiterReadRepository.GetSubscriptionIdByPlanName(planName);
            if (subscriptionId == null) throw new InvalidOperationException("Subscription not found for the plan name.");

            var recruiter = await _recruiterReadRepository.GetByIdAsync(recruiterId);
            if (recruiter == null) throw new InvalidOperationException("Recruiter not found.");

            if (recruiter.Subscription != null && recruiter.SubscriptionStartDate != default(DateTime))
            {
                var subscriptionEndDate = recruiter.SubscriptionStartDate.AddDays(30);
                if (DateTime.Now < subscriptionEndDate)
                {
                    throw new InvalidOperationException("The user is already subscribed, and the subscription is not yet expired.");
                }
            }
            var appUserId = Guid.Parse(recruiter.AppUserId);

            var subscription = await _subscriptionReadRepository.GetByIdAsync(subscriptionId.Value);
            if (subscription == null) throw new InvalidOperationException("Subscription object not found.");

            recruiter.Subscription = subscription;
            recruiter.SubscriptionStartDate = DateTime.Now;

            _recruiterWriteRepository.Update(recruiter);
            await _recruiterWriteRepository.SaveChangesAsync();

            var title = "Subscription Updated";
            var message = "Congratulations! Your subscription has been successfully updated. Thank you for choosing our service.";
            BackgroundJob.Schedule<INotificationService>(x => x.CreateAsync(appUserId, title, message), TimeSpan.FromSeconds(10));
        }
    }
}
