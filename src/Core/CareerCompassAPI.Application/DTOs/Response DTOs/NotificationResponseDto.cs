using CareerCompassAPI.Domain.Enums;

namespace CareerCompassAPI.Application.DTOs.Response_DTOs
{
    public record NotificationResponseDto(Guid id, string title, string message, DateTime dateCreated, ReadStatus ReadStatus);
}
