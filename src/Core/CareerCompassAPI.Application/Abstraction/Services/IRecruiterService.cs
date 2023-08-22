using CareerCompassAPI.Application.DTOs.Recruiter_DTOs;
using CareerCompassAPI.Domain.Entities;

namespace CareerCompassAPI.Application.Abstraction.Services
{
    public interface IRecruiterService
    {
        Task<RecruiterGetDto> GetRecruiterByUserId(Guid userId);
        bool IsSubscriptionActive(Recruiter recruiter);
    }
}
