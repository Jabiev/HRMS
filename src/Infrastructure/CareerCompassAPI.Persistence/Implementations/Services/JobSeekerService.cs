using CareerCompassAPI.Application.Abstraction.Repositories.IJobSeekerRepositories;
using CareerCompassAPI.Application.Abstraction.Services;
using CareerCompassAPI.Application.DTOs.JobSeeker_DTOs;

namespace CareerCompassAPI.Persistence.Implementations.Services
{
    public class JobSeekerService : IJobSeekerService
    {
        private readonly IJobSeekerReadRepository _jobSeekerReadRepository;

        public JobSeekerService(IJobSeekerReadRepository jobSeekerReadRepository)
        {
            _jobSeekerReadRepository = jobSeekerReadRepository;
        }

        public async Task<JobSeekerGetDto> GetByUserId(Guid userId)
        {
            var response = await _jobSeekerReadRepository.GetByUserIdAsync(userId);
            JobSeekerGetDto jobSeeker = new(
                id: response.Id,
                firstName: response.FirstName,
                lastName: response.LastName);
            return jobSeeker;
        }
    }
}
