using CareerCompassAPI.Application.Abstraction.Repositories.IJobSeekerRepositories;
using CareerCompassAPI.Domain.Entities;
using CareerCompassAPI.Persistence.Contexts;

namespace CareerCompassAPI.Persistence.Implementations.Repositories.JobSeekerRepositories
{
    public class JobSeekerReadRepository : ReadRepository<JobSeekers>, IJobSeekerReadRepository
    {
        public JobSeekerReadRepository(CareerCompassDbContext context) : base(context)
        {
        }
    }
}
