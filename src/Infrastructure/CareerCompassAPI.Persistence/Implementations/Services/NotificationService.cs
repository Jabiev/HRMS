using CareerCompassAPI.Application.Abstraction.Repositories.INotificationRepositories;
using CareerCompassAPI.Application.Abstraction.Services;
using CareerCompassAPI.Application.DTOs.Response_DTOs;
using CareerCompassAPI.Domain.Entities;
using CareerCompassAPI.Domain.Enums;
using CareerCompassAPI.Domain.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

public class NotificationService : INotificationService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly INotificationWriteRepository _notificationWriteRepository;
    private readonly INotificationReadRepository _notificationReadRepository;

    public NotificationService(UserManager<AppUser> userManager,
                               IHttpContextAccessor httpContextAccessor,
                               INotificationWriteRepository notificationWriteRepository,
                               INotificationReadRepository notificationReadRepository)
    {
        _userManager = userManager;
        _httpContextAccessor = httpContextAccessor;
        _notificationWriteRepository = notificationWriteRepository;
        _notificationReadRepository = notificationReadRepository;
    }

    public async Task<NotificationResponseDto> CreateAsync(Guid userId, string title, string message)
    {
        var notification = new Notification
        {
            Title = title,
            Message = message,
            UserId = userId,
            ReadStatus = ReadStatus.Unread
        };
        await _notificationWriteRepository.AddAsync(notification);
        await _notificationWriteRepository.SaveChangesAsync();

        return new NotificationResponseDto(title, message, DateTime.UtcNow);
    }


    public async Task<IEnumerable<NotificationResponseDto>> GetNotificationsAsync(Guid userId)
    {
        var notifications = await _notificationReadRepository.GetByUserIdAsync(userId);
        return notifications.Select(n => new NotificationResponseDto(n.Title, n.Message, n.DateCreated));
    }


}
