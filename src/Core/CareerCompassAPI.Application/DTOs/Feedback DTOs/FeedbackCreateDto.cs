namespace CareerCompassAPI.Application.DTOs.Feedback_DTOs
{
    public record FeedbackCreateDto(string firstName, string lastName, string description, string imageUrl, string jobTitle);
}
