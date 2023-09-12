namespace CareerCompassAPI.Application.DTOs.Dashboard_DTOs
{
    public record PendingReviewsDto(Guid reviewId, string email, string title, string description, decimal rating);
}
