using CareerCompassAPI.Domain.Entities;

namespace CareerCompassAPI.Application.Abstraction.Repositories.IRecruiterRepositories
{
    public interface IRecruiterReadRepository:IReadRepository<Recruiter>
    {
        Task<Recruiter?> GetByUserIdAsync(Guid userId);
        Task<Guid?> GetSubscriptionIdByPlanName(string planName);
    }
}
