using CareerCompassAPI.Application.Abstraction.Repositories.IJobApplicationRepositories;
using CareerCompassAPI.Domain.Entities;
using CareerCompassAPI.Persistence.Contexts;

namespace CareerCompassAPI.Persistence.Implementations.Repositories.JobApplicationRepositories
{
    public class JobApplicationWriteRepository : WriteRepository<JobApplications>, IJobApplicationWriteRepository
    {
        public JobApplicationWriteRepository(CareerCompassDbContext context) : base(context)
        {
        }
    }
}
