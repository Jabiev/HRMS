using CareerCompassAPI.Application.Abstraction.Repositories.ITeamRepositories;
using CareerCompassAPI.Domain.Entities;
using CareerCompassAPI.Persistence.Contexts;

namespace CareerCompassAPI.Persistence.Implementations.Repositories.TeamRepositories
{
    public class TeamWriteRepository : WriteRepository<TeamMember>, ITeamWriteRepository
    {
        public TeamWriteRepository(CareerCompassDbContext context) : base(context)
        {
        }
    }
}
