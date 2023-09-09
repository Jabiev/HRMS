using CareerCompassAPI.Application.Abstraction.Services;
using Microsoft.AspNetCore.SignalR;

namespace CareerCompassAPI.SignalR.Hubs
{
    public class NotificationsHub : Hub
    {
        private readonly INotificationService _notification;
        public async Task SendNotificationMarkedAsRead(Guid notificationId)
        {
            await Clients.All.SendAsync("NotificationMarkedAsRead", notificationId);
        }
    }
}
