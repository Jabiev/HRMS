using CareerCompassAPI.Application.DTOs.FeatureAccess_DTO;
using CareerCompassAPI.Application.DTOs.Recruiter_DTOs;
using CareerCompassAPI.Domain.Entities;

namespace CareerCompassAPI.Application.Abstraction.Services
{
    public interface IRecruiterService
    {
        Task<RecruiterGetDto> GetRecruiterByUserId(Guid userId);
        Task<Subscriptions> GetSubscriptionForRecruiter(Guid appUserId);
        Task<FeatureAccessGetDto> GetAvailableFeaturesForRecruiter(Guid recruiterId);
        Task<GetRecruiterNameDto> GetRecruiterName(string appUserId);

    }
}
