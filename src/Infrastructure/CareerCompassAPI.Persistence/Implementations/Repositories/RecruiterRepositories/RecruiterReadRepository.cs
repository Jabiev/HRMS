using CareerCompassAPI.Application.Abstraction.Repositories.IRecruiterRepositories;
using CareerCompassAPI.Domain.Entities;
using CareerCompassAPI.Persistence.Contexts;

namespace CareerCompassAPI.Persistence.Implementations.Repositories.RecruiterRepositories
{
    public class RecruiterReadRepository : ReadRepository<Recruiter>, IRecruiterReadRepository
    {
        public RecruiterReadRepository(CareerCompassDbContext context) : base(context)
        {
        }
    }
}
