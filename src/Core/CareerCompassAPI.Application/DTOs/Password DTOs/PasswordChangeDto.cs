namespace CareerCompassAPI.Application.DTOs.Password_DTOs
{
    public record PasswordChangeDto(string oldPassword, string newPassword, string confirmNewPassword);
}
