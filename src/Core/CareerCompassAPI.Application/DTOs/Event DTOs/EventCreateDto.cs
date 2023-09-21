namespace CareerCompassAPI.Application.DTOs.Event_DTOs
{
    public record EventCreateDto(string userId, string title, DateTime startDate, DateTime endDate);
}
