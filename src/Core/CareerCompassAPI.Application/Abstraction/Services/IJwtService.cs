using CareerCompassAPI.Application.DTOs.Response_DTOs;
using CareerCompassAPI.Domain.Identity;

namespace CareerCompassAPI.Application.Abstraction.Services
{
    public interface IJwtService
    {
        Task<TokenResponseDto> CreateJwtToken(AppUser user);
    }
}
