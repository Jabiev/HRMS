using CareerCompassAPI.Application.DTOs.Application_DTOs;

namespace CareerCompassAPI.Application.Abstraction.Services
{
    public interface IApplicationService
    {
        Task<int> CreateAsync(ApplicationCreateDto applicationCreateDto);
        Task<List<ApplicantsGetDto>> GetApplicationsByAppUserId(string appUserId);
        Task UpdateAsync(ApplicationStatusUpdateDto applicationStatusUpdateDto);
        Task<List<ApprovedApplicantGetDto>> GetApprovedApplicantsByAppUserId(string appUserId);
        Task Remove(Guid applicationId);
    }
}
