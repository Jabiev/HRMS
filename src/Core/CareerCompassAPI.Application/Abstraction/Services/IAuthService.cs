using CareerCompassAPI.Application.DTOs.Auth_DTOs;

namespace CareerCompassAPI.Application.Abstraction.Services
{
    public interface IAuthService
    {
        Task Register(UserRegisterDto userRegisterDto);

    }
}
