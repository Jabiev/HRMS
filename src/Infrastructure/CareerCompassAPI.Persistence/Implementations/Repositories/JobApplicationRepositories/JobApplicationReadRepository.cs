using CareerCompassAPI.Application.Abstraction.Repositories.IJobApplicationRepositories;
using CareerCompassAPI.Domain.Entities;
using CareerCompassAPI.Persistence.Contexts;

namespace CareerCompassAPI.Persistence.Implementations.Repositories.JobApplicationRepositories
{
    public class JobApplicationReadRepository : ReadRepository<JobApplications>, IJobApplicationReadRepository
    {
        public JobApplicationReadRepository(CareerCompassDbContext context) : base(context)
        {
        }
    }
}
