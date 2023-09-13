using CareerCompassAPI.Application.DTOs.Company_DTOs;
using CareerCompassAPI.Application.DTOs.JobSeeker_DTOs;

namespace CareerCompassAPI.Application.Abstraction.Services
{
    public interface IJobSeekerService
    {
        Task<JobSeekerGetDto> GetByUserId(Guid userId);
        Task CreateAsync(JobSeekerCreateDto dto, string jobseekerAppUserId);
        Task<List<JobseekerApprovedGetDto>> GetApprovedPositionsByAppUserId(string appUserId);
        Task UploadLogoAsync(string appUserId, JobseekerAvatarUploadDto avatarUploadDto);
    }
}
