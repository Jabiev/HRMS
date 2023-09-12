using CareerCompassAPI.Application.Abstraction.Services;
using CareerCompassAPI.Application.DTOs.AppUser_DTOs;
using CareerCompassAPI.Application.DTOs.Dashboard_DTOs;
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

        public async Task ChangeUserRole(ChangeUserRoleDto changeUserRoleDto)
        {
            var user = await _userManager.FindByIdAsync(changeUserRoleDto.appUserId);

            if (user == null)
            {
                throw new NotFoundException("User not found.");
            }

            if (changeUserRoleDto.newRole != "Admin" && changeUserRoleDto.newRole != "Master")
            {
                throw new Exception("Invalid role.");
            }

            var roles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, roles); 

            await _userManager.AddToRoleAsync(user, changeUserRoleDto.newRole); 
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

            // Manually delete the messages
            var messagesToDelete = await _context.Messages
                                        .Where(m => m.Sender.Id == appUserId || m.Receiver.Id == appUserId)
                                        .ToListAsync();

            _context.Messages.RemoveRange(messagesToDelete);
            await _context.SaveChangesAsync();

            // Proceed with deleting the user
            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                throw new Exception("Failed to delete the user");  // Custom Exception
            }
        }

    }
}
