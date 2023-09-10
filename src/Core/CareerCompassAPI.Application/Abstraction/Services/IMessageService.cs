using CareerCompassAPI.Application.DTOs.Message_DTOs;

namespace CareerCompassAPI.Application.Abstraction.Services
{
    public interface IMessageService
    {
        Task CreateAsync(MessageCreateDto messageCreateDto);
        Task<List<GetUnreadMessagesDto>> GetUnreadMessagesAsync(string senderId, string receiverId);
    }
}
