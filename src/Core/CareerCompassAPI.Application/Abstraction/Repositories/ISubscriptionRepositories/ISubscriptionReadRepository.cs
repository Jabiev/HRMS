using CareerCompassAPI.Application.DTOs.Subscription_DTOs;
using CareerCompassAPI.Domain.Entities;

namespace CareerCompassAPI.Application.Abstraction.Repositories.ISubscriptionRepository
{
    public interface ISubscriptionReadRepository : IReadRepository<Subscriptions>
    {
        Task<Subscriptions?> GetByIdAsync(Guid subscriptionId);
        Task<List<Subscriptions>> GetAllAsync();
    }
}
