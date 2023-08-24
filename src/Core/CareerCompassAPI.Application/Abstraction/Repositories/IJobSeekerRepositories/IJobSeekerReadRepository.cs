using CareerCompassAPI.Domain.Entities;

namespace CareerCompassAPI.Application.Abstraction.Repositories.IJobSeekerRepositories
{
    public interface IJobSeekerReadRepository:IReadRepository<JobSeeker>
    {
        Task<JobSeeker?> GetByUserIdAsync(Guid userId);
    }
}
