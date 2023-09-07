namespace CareerCompassAPI.Application.DTOs.Review_DTOs
{
    public record ReviewCreateDto(string appUserId, string title, string description, Guid companyId);
}
