using CareerCompassAPI.Application.Abstraction.Repositories.IRecruiterRepositories;
using CareerCompassAPI.Domain.Entities;
using CareerCompassAPI.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace CareerCompassAPI.Persistence.Implementations.Repositories.RecruiterRepositories
{
    public class RecruiterReadRepository : ReadRepository<Recruiter>, IRecruiterReadRepository
    {
        private readonly CareerCompassDbContext _context;
        public RecruiterReadRepository(CareerCompassDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Recruiter?> GetByUserIdAsync(Guid userId)
        {
            return await _context.Recruiters
                .Include(r => r.AppUser) 
                .Include(r => r.Company)
                .Include(r => r.Subscription)
                .FirstOrDefaultAsync(r => r.AppUserId == userId.ToString());
        }

        public async Task<Guid?> GetSubscriptionIdByPlanName(string planName)
        {
            var subscription = await _context.Subscriptions.Where(s => s.Name == planName).FirstOrDefaultAsync();
            return subscription?.Id;
        }
    }
}
