using CareerCompassAPI.Application.Abstraction.Repositories.ITeamRepositories;
using CareerCompassAPI.Domain.Entities;
using CareerCompassAPI.Persistence.Contexts;

namespace CareerCompassAPI.Persistence.Implementations.Repositories.TeamRepositories
{
    public class TeamReadRepository : ReadRepository<TeamMember>, ITeamReadRepository
    {
        public TeamReadRepository(CareerCompassDbContext context) : base(context)
        {
        }
    }
}
