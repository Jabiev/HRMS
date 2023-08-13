using CareerCompassAPI.Application.Abstraction.Repositories.INotificationRepositories;
using CareerCompassAPI.Domain.Entities;
using CareerCompassAPI.Persistence.Contexts;

namespace CareerCompassAPI.Persistence.Implementations.Repositories.NotificationRepositories
{
    public class NotificationWriteRepository : WriteRepository<Notification>, INotificationWriteRepository
    {
        public NotificationWriteRepository(CareerCompassDbContext context) : base(context)
        {
        }
    }
}
