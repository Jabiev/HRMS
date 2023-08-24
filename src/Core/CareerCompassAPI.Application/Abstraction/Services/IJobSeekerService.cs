using CareerCompassAPI.Application.DTOs.JobSeeker_DTOs;

namespace CareerCompassAPI.Application.Abstraction.Services
{
    public interface IJobSeekerService
    {
        Task<JobSeekerGetDto> GetByUserId(Guid userId);
    }
}
