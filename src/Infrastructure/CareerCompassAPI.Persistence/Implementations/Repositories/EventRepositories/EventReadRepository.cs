using CareerCompassAPI.Application.Abstraction.Repositories.IEventRepositories;
using CareerCompassAPI.Domain.Entities;
using CareerCompassAPI.Persistence.Contexts;

namespace CareerCompassAPI.Persistence.Implementations.Repositories.EventRepositories
{
    public class EventReadRepository : ReadRepository<Event>, IEventReadRepository
    {
        public EventReadRepository(CareerCompassDbContext context) : base(context)
        {
        }
    }
}
