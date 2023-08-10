using CareerCompassAPI.Application.Abstraction.Repositories.IRecruiterRepositories;
using CareerCompassAPI.Domain.Entities;
using CareerCompassAPI.Persistence.Contexts;

namespace CareerCompassAPI.Persistence.Implementations.Repositories.RecruiterRepositories
{
    public class RecruiterWriteRepository : WriteRepository<Recruiter>, IRecruiterWriteRepository
    {
        public RecruiterWriteRepository(CareerCompassDbContext context) : base(context)
        {
        }
    }
}
