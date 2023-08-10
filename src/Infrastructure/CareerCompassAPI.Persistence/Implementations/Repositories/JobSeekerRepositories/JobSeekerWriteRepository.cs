using CareerCompassAPI.Application.Abstraction.Repositories.IJobSeekerRepositories;
using CareerCompassAPI.Domain.Entities;
using CareerCompassAPI.Persistence.Contexts;

namespace CareerCompassAPI.Persistence.Implementations.Repositories.JobSeekerRepositories
{
    public class JobSeekerWriteRepository : WriteRepository<JobSeeker>, IJobSeekerWriteRepository
    {
        public JobSeekerWriteRepository(CareerCompassDbContext context) : base(context)
        {
        }
    }
}
