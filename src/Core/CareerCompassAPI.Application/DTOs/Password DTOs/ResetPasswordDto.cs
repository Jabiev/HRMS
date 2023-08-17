namespace CareerCompassAPI.Application.DTOs.Password_DTOs
{
    public record ResetPasswordDto(string userId, string token, string password, string confirmPassword);
}
