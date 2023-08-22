using CareerCompassAPI.Application.Abstraction.Services;
using CareerCompassAPI.Application.DTOs.Payment_DTOs;
using CareerCompassAPI.Domain.Stripe;
using Stripe;
using Stripe.Checkout;

namespace CareerCompassAPI.Persistence.Implementations.Services
{
    public class StripeAppService:IStripeAppService
    {
        private readonly ChargeService _chargeService;
        private readonly CustomerService _customerService;
        private readonly TokenService _tokenService;
        private readonly SessionService _sessionService;
        public StripeAppService(
            ChargeService chargeService,
            CustomerService customerService,
            TokenService tokenService,
            SessionService sessionService)
        {
            _chargeService = chargeService;
            _customerService = customerService;
            _tokenService = tokenService;
            _sessionService = sessionService;
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
            };

            var session = await _sessionService.CreateAsync(options, cancellationToken: ct);

            return session.Id;
        }

    }
}
