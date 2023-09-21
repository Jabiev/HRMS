using CareerCompassAPI.Application.Abstraction.Repositories.IEventRepositories;
using CareerCompassAPI.Domain.Entities;
using CareerCompassAPI.Persistence.Contexts;

namespace CareerCompassAPI.Persistence.Implementations.Repositories.EventRepositories
{
    public class EventWriteRepository : WriteRepository<Event>, IEventWriteRepository
    {
        public EventWriteRepository(CareerCompassDbContext context) : base(context)
        {
        }
    }
}
