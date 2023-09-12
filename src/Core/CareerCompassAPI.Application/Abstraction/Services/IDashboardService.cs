using CareerCompassAPI.Application.DTOs.AppUser_DTOs;

namespace CareerCompassAPI.Application.Abstraction.Services
{
    public interface IDashboardService
    {
        Task<List<AppUserGetDto>> GetAllAsync();
        Task RemoveUser(string appUserId);
    }
}
