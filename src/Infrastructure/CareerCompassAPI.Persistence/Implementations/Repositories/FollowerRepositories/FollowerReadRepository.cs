using CareerCompassAPI.Application.Abstraction.Repositories.IFollowerRepositories;
using CareerCompassAPI.Domain.Entities;
using CareerCompassAPI.Persistence.Contexts;

namespace CareerCompassAPI.Persistence.Implementations.Repositories.FollowerRepositories
{
    public class FollowerReadRepository : ReadRepository<Follower>, IFollowerReadRepository
    {
        public FollowerReadRepository(CareerCompassDbContext context) : base(context)
        {
        }
    }
}
