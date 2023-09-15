namespace CareerCompassAPI.Application.DTOs.Dashboard_DTOs
{
    public record SubscriptionGetDto(Guid id, string name, decimal price, int postLimit);
}
