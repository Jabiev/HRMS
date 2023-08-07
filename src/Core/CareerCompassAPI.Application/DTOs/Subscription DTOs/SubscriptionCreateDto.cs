namespace CareerCompassAPI.Application.DTOs.Subscription_DTOs
{
    public record SubscriptionCreateDto(string? Name, decimal Price, int PostLimit);
}
