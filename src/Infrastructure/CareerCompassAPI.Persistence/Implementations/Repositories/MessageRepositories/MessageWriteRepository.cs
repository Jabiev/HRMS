using CareerCompassAPI.Application.Abstraction.Repositories.IMessageRepositories;
using CareerCompassAPI.Persistence.Contexts;

namespace CareerCompassAPI.Persistence.Implementations.Repositories.MessageRepositories
{
    public class MessageWriteRepository : WriteRepository<Domain.Entities.Message>, IMessageWriteRepository
    {
        public MessageWriteRepository(CareerCompassDbContext context) : base(context)
        {
        }
    }
}
