using CareerCompassAPI.Domain.Enums;

namespace CareerCompassAPI.Application.DTOs.Review_DTOs
{
    public record ReviewCreateDto(string appUserId, string title, string description, decimal rating, ReviewStatus status, Guid companyId);
}
