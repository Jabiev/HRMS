using CareerCompassAPI.Domain.Entities;

namespace CareerCompassAPI.Application.DTOs.Recruiter_DTOs
{
    public record RecruiterGetDto(
        string AppUserId,
        Guid? CompanyId,
        string? FirstName,
        string? LastName,
        Guid? JobLocationId,
        Subscriptions Subscription
    );
}
