using CareerCompassAPI.Application.DTOs.Recruiter_DTOs;

namespace CareerCompassAPI.Application.Abstraction.Services
{
    public interface IRecruiterService
    {
        Task<RecruiterGetDto> GetRecruiterByUserId(Guid userId);
    }
}
