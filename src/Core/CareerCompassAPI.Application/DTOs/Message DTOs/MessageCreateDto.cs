namespace CareerCompassAPI.Application.DTOs.Message_DTOs
{
    public record MessageCreateDto(string senderId, string receiverId, string content, bool isRead, string messageType);
}
