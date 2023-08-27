using CareerCompassAPI.Application.DTOs.Application_DTOs;

namespace CareerCompassAPI.Application.Abstraction.Services
{
    public interface IApplicationService
    {
        Task CreateAsync(ApplicationCreateDto applicationCreateDto);
    }
}
