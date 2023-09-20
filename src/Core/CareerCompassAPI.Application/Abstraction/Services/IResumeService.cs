using CareerCompassAPI.Application.DTOs.Resume_DTOs;

namespace CareerCompassAPI.Application.Abstraction.Services
{
    public interface IResumeService
    {
        Task<Guid> CreateResume(ResumeCreateDto resumeCreateDto);
        Task<List<ResumeGetDto>> GetAllResumes();
        Task<ResumeGetDto> GetResumeById(Guid id);
    }
}
