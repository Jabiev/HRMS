using CareerCompassAPI.Application.Abstraction.Repositories.INotificationRepositories;
using CareerCompassAPI.Domain.Entities;
using CareerCompassAPI.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace CareerCompassAPI.Persistence.Implementations.Repositories.NotificationRepositories
{
    public class NotificationReadRepository : ReadRepository<Notification>, INotificationReadRepository
    {
        private readonly CareerCompassDbContext _context;
        public NotificationReadRepository(CareerCompassDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Notification>> GetByUserIdAsync(Guid userId)
        {
            return await _context.Notifications
            .Where(n => n.UserId == userId)
            .ToListAsync();
        }
    }
}
