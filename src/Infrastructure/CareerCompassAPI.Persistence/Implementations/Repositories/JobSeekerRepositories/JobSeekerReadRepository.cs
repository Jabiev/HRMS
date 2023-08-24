using CareerCompassAPI.Application.Abstraction.Repositories.IJobSeekerRepositories;
using CareerCompassAPI.Domain.Entities;
using CareerCompassAPI.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace CareerCompassAPI.Persistence.Implementations.Repositories.JobSeekerRepositories
{
    public class JobSeekerReadRepository : ReadRepository<JobSeeker>, IJobSeekerReadRepository
    {
        private readonly CareerCompassDbContext _context;

        public JobSeekerReadRepository(CareerCompassDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<JobSeeker?> GetByUserIdAsync(Guid userId)
        {
            return await _context.JobSeekers
                .Where(js => js.AppUserId == userId.ToString())
                .Select(js => new JobSeeker
                {
                    Id = js.Id,
                    FirstName = js.FirstName,
                    LastName = js.LastName
                })
                .FirstOrDefaultAsync();
        }
    }
}
