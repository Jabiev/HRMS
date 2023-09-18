namespace CareerCompassAPI.Application.Abstraction.Services
{
    public interface IHangFireService
    {
        Task CheckSubscriptions();
        Task DeleteOldMessages();
        Task DeleteOldNotifications();
    }
}
