using CareerCompassAPI.Application.DTOs.Payment_DTOs;
using CareerCompassAPI.Domain.Stripe;
using Stripe;
using Stripe.Checkout;
using System.Numerics;

namespace CareerCompassAPI.Application.Abstraction.Services
{
    public interface IStripeAppService
    {
        Task<StripeCustomer> AddStripeCustomerAsync(AddStripeCustomer customer, CancellationToken ct);
        Task<StripePayment> AddStripePaymentAsync(AddStripePayment payment, CancellationToken ct);
        Task<string> CreateCheckoutSessionAsync(PlanDTO plan, CancellationToken ct);
        Task<Session> RetrieveSessionAsync(string sessionId);
        Task UpdateSubscriptionAsync(string sessionId,string planName);
        Task<string> CreateCheckoutSessionForResumeAsync(JobSeekerResumeCreateDto jobSeekerResume, CancellationToken ct);
    }
}
