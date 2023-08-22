using CareerCompassAPI.Application.Abstraction.Repositories.ISubscriptionRepository;
using CareerCompassAPI.Domain.Entities;
using CareerCompassAPI.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace CareerCompassAPI.Persistence.Implementations.Repositories.SubscriptionRepositories
{
    public class SubscriptionReadRepository : ReadRepository<Subscriptions>, ISubscriptionReadRepository
    {
        private readonly CareerCompassDbContext _context;
        public SubscriptionReadRepository(CareerCompassDbContext context) : base(context)
        {
            _context = context;
        }

        async Task<Subscriptions?> ISubscriptionReadRepository.GetByIdAsync(Guid subscriptionId)
        {
            var subscription = await _context.Subscriptions.Where(s => s.Id == subscriptionId).FirstOrDefaultAsync();
            return subscription;
        }
    }
}
