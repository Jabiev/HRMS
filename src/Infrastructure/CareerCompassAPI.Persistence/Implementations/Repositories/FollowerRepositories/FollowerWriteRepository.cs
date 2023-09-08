using CareerCompassAPI.Application.Abstraction.Repositories.IFollowerRepositories;
using CareerCompassAPI.Domain.Entities;
using CareerCompassAPI.Persistence.Contexts;

namespace CareerCompassAPI.Persistence.Implementations.Repositories.FollowerRepositories
{
    public class FollowerWriteRepository : WriteRepository<Follower>, IFollowerWriteRepository
    {
        public FollowerWriteRepository(CareerCompassDbContext context) : base(context)
        {
        }
    }
}
