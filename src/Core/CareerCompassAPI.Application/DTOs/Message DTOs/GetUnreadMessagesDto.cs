namespace CareerCompassAPI.Application.DTOs.Message_DTOs
{
    public record GetUnreadMessagesDto(string senderId, string receiverId, string content, string messageType);
}
