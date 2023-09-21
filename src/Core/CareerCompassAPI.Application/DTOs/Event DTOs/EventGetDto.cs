namespace CareerCompassAPI.Application.DTOs.Event_DTOs
{
    public record EventGetDto(Guid id, string title, DateTime start, DateTime end);
}
