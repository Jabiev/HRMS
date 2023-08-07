using CareerCompassAPI.Application.Abstraction.Repositories.ISubscriptionRepository;
using CareerCompassAPI.Domain.Entities;
using CareerCompassAPI.Persistence.Contexts;

namespace CareerCompassAPI.Persistence.Implementations.Repositories.SubscriptionRepositories
{
    public class SubscriptionWriteRepository : WriteRepository<Subscriptions>, ISubscriptionWriteRepository
    {
        public SubscriptionWriteRepository(CareerCompassDbContext context) : base(context)
        {
        }
    }
}
