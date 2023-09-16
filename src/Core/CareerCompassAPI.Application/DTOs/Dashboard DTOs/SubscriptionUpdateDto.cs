namespace CareerCompassAPI.Application.DTOs.Dashboard_DTOs
{
    public record SubscriptionUpdateDto(Guid id,string name, decimal price, int postLimit);
}
