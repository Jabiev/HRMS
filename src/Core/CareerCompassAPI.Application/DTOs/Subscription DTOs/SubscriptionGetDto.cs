namespace CareerCompassAPI.Application.DTOs.Subscription_DTOs
{
    public record SubscriptionGetDto(string name, decimal price, int postLimit, bool isChatAvailable, bool isPlannerAvailable, bool isVideoAvailable);
}
