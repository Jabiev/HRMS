using CareerCompassAPI.Application.Abstraction.Repositories.IMessageRepositories;
using CareerCompassAPI.Persistence.Contexts;

namespace CareerCompassAPI.Persistence.Implementations.Repositories.MessageRepositories
{
    public class MessageReadRepository : ReadRepository<Domain.Entities.Message>, IMessageReadRepository
    {
        public MessageReadRepository(CareerCompassDbContext context) : base(context)
        {
        }
    }
}
