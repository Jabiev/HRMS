using CareerCompassAPI.Application.DTOs.Auth_DTOs;
using CareerCompassAPI.Application.DTOs.Response_DTOs;

namespace CareerCompassAPI.Application.Abstraction.Services
{
    public interface IAuthService
    {
        Task Register(UserRegisterDto userRegisterDto);
        Task<TokenResponseDto> Login(UserSignInDto userSignInDto);
        Task<TokenResponseDto> ValidateRefreshToken(string refreshToken);
    }
}
