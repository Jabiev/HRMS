namespace CareerCompassAPI.Application.DTOs.Dashboard_DTOs
{
    public record UserFeedbacksGetDto(Guid feedbackId, string firstName, string lastName, string description, string imageUrl, string position, bool isActive);
}
