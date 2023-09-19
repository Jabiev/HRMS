namespace CareerCompassAPI.Application.DTOs.Auth_DTOs
{
    public record UserDetailsGetDto(string FirstName, string LastName, string Email, string? SubscriptionName);

}
