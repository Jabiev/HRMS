using CareerCompassAPI.Domain.Entities;

namespace CareerCompassAPI.Application.DTOs.Recruiter_DTOs
{
    public record RecruiterGetDto(
        Guid id,
        string AppUserId,
        Guid? CompanyId,
        string? FirstName,
        string? LastName,
        Guid SubscriptionId,
        DateTime SubscriptionStartDate
    );
}
