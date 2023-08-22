using CareerCompassAPI.Application.DTOs.Payment_DTOs;
using CareerCompassAPI.Domain.Stripe;
using Stripe;
using System.Numerics;

namespace CareerCompassAPI.Application.Abstraction.Services
{
    public interface IStripeAppService
    {
        Task<StripeCustomer> AddStripeCustomerAsync(AddStripeCustomer customer, CancellationToken ct);
        Task<StripePayment> AddStripePaymentAsync(AddStripePayment payment, CancellationToken ct);
        Task<string> CreateCheckoutSessionAsync(PlanDTO plan, CancellationToken ct);
    }
}
