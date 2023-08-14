using CareerCompassAPI.Application.DTOs.Response_DTOs;

namespace CareerCompassAPI.Application.Abstraction.Services
{
    public interface INotificationService
    {
        Task<NotificationResponseDto> CreateAsync(Guid userId ,string title,string message);
        Task<IEnumerable<NotificationResponseDto>> GetNotificationsAsync(Guid userId);
        Task MarkAsReadAsync(Guid notificationId);
    }
}
