using CareerCompassAPI.Application.DTOs.Auth_DTOs;

namespace CareerCompassAPI.Application.Abstraction.Services
{
    public interface IUserService
    {
        Task<UserDetailsGetDto> GetDetailsAsync(string userId);
    }
}
