using CareerCompassAPI.Domain.Enums;

namespace CareerCompassAPI.Application.DTOs.Auth_DTOs
{
    public record UserRegisterDto(string firstName, string lastName, string email, string password, string phoneNumber, Roles role);
}
