using CareerCompassAPI.Application.DTOs.Subscription_DTOs;

namespace CareerCompassAPI.Application.Abstraction.Services
{
    public interface ISubscriptionService
    {
        Task CreateAsync(SubscriptionCreateDto subscriptionCreateDto);
    }
}
