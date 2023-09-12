using CareerCompassAPI.Application.Abstraction.Services;
using CareerCompassAPI.Application.DTOs.AppUser_DTOs;
using CareerCompassAPI.Domain.Identity;
using CareerCompassAPI.Persistence.Contexts;
using CareerCompassAPI.Persistence.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CareerCompassAPI.Persistence.Implementations.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly CareerCompassDbContext _context;

        public DashboardService(UserManager<AppUser> userManager, CareerCompassDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<List<AppUserGetDto>> GetAllAsync()
        {
            var appUsers = new List<AppUserGetDto>();

            var users = _userManager.Users.ToList();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);

                appUsers.Add(new AppUserGetDto(
                    user.Id,
                    user.UserName,
                    user.Email,
                    user.PhoneNumber,
                    roles.FirstOrDefault() ?? "No Role Assigned"
                ));
            }
            return appUsers;
        }

        public async Task RemoveUser(string appUserId)
        {
            if (string.IsNullOrEmpty(appUserId))
            {
                throw new NotFoundException("Empty value cannot be passed as an argument");
            }

            var user = await _userManager.FindByIdAsync(appUserId);
            if (user == null)
            {
                throw new NotFoundException("User with given parameters does not exist");
            }

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                throw new Exception("Failed to delete the user");  // Custom Exception
            }
        }

    }
}
