using CareerCompassAPI.Application.DTOs.AppUser_DTOs;
using CareerCompassAPI.Application.DTOs.Dashboard_DTOs;

namespace CareerCompassAPI.Application.Abstraction.Services
{
    public interface IDashboardService
    {
        Task<List<AppUserGetDto>> GetAllAsync(string? searchQuery);
        Task RemoveUser(string appUserId);
        Task ChangeUserRole(ChangeUserRoleDto changeUserRoleDto);
        Task<List<CompaniesListGetDto>> GetAllCompaniesAsync(string? sortOrder, string? searchQuery);
        Task RemoveCompany(Guid companyId);
    }
}
