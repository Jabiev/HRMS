using CareerCompassAPI.Application.Abstraction.Repositories.ICompanyRepositories;
using CareerCompassAPI.Application.Abstraction.Repositories.IRecruiterRepositories;
using CareerCompassAPI.Application.Abstraction.Repositories.IReviewRepositories;
using CareerCompassAPI.Application.Abstraction.Services;
using CareerCompassAPI.Application.DTOs.AppUser_DTOs;
using CareerCompassAPI.Application.DTOs.Dashboard_DTOs;
using CareerCompassAPI.Domain.Enums;
using CareerCompassAPI.Domain.Identity;
using CareerCompassAPI.Persistence.Contexts;
using CareerCompassAPI.Persistence.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace CareerCompassAPI.Persistence.Implementations.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly CareerCompassDbContext _context;
        private readonly ICompanyReadRepository _companyReadRepository;
        private readonly ICompanyWriteRepository _companyWriteRepository;
        private readonly IRecruiterReadRepository _recruiterReadRepository;
        private readonly IRecruiterWriteRepository _recruiterWriteRepository;
        private readonly IReviewReadRepository _reviewReadRepository;

        public DashboardService(UserManager<AppUser> userManager,
                                CareerCompassDbContext context,
                                ICompanyReadRepository companyReadRepository,
                                ICompanyWriteRepository companyWriteRepository,
                                IRecruiterReadRepository recruiterReadRepository,
                                IRecruiterWriteRepository recruiterWriteRepository,
                                IReviewReadRepository reviewReadRepository)
        {
            _userManager = userManager;
            _context = context;
            _companyReadRepository = companyReadRepository;
            _companyWriteRepository = companyWriteRepository;
            _recruiterReadRepository = recruiterReadRepository;
            _recruiterWriteRepository = recruiterWriteRepository;
            _reviewReadRepository = reviewReadRepository;
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
        public async Task<List<AppUserGetDto>> GetAllAsync(string searchQuery = "")
        {
            var appUsers = new List<AppUserGetDto>();
            IQueryable<AppUser> queryableUsers = _userManager.Users;

            if (!string.IsNullOrEmpty(searchQuery))
            {
                queryableUsers = queryableUsers.Where(
                    u => u.UserName.ToLower().Contains(searchQuery.ToLower()) ||
                         u.Email.ToLower().Contains(searchQuery.ToLower()) ||
                         u.PhoneNumber.ToLower().Contains(searchQuery.ToLower())
                );
            }

            var users = await queryableUsers.ToListAsync();  

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


        public async Task<List<CompaniesListGetDto>> GetAllCompaniesAsync(string? sortOrders, string? searchQuery)
        {
         
            var companiesQuery = await _context.Companies
                .Include(c => c.Followers)
                .Include(c => c.Reviews)
                .Include(c => c.Details)
                    .ThenInclude(cd => cd.Location)
                .ToListAsync();

            if (!string.IsNullOrEmpty(searchQuery))
            {
                companiesQuery = companiesQuery.Where(c =>
                    c.Name.ToLower().Contains(searchQuery.ToLower()) ||
                    c.Details.Location.Location.ToLower().Contains(searchQuery.ToLower())
                ).ToList();
            }

            List<CompaniesListGetDto> sortedCompanies = companiesQuery
                .Select(c => new CompaniesListGetDto(
                    c.Id,
                    c.Name,
                    c.Followers.Count,
                    c.Reviews.Count,
                    c.Details.Location.Location
                ))
                .ToList();

            if (string.IsNullOrEmpty(sortOrders))
            {
                return sortedCompanies;
            }
            var sorts = sortOrders.Split('|');
            foreach (var sort in sorts)
            {
                var parts = sort.Split('_');
                if (parts.Length != 2)
                {
                    continue; 
                }

                var field = parts[0].ToLower();
                var direction = parts[1].ToLower();

                switch (field)
                {
                    case "followers":
                        sortedCompanies = direction == "asc" ?
                            sortedCompanies.OrderBy(c => c.followersCount).ToList() :
                            sortedCompanies.OrderByDescending(c => c.followersCount).ToList();
                        break;
                    case "reviews":
                        sortedCompanies = direction == "asc" ?
                            sortedCompanies.OrderBy(c => c.reviewsCount).ToList() :
                            sortedCompanies.OrderByDescending(c => c.reviewsCount).ToList();
                        break;
                    default:
                        break;
                }
            }
            return sortedCompanies;
        }

        public async Task<List<PendingReviewsDto>> GetAllPendingReviews()
        {
            var pendingReviews = _reviewReadRepository.GetAllByExpression(
                r => r.Status == ReviewStatus.Pending,
                int.MaxValue, 
                0,
                true,
                "JobSeeker.AppUser"
            );

            var pendingReviewList = await pendingReviews.ToListAsync();

            return pendingReviewList.Select(r =>
                new PendingReviewsDto(r.Id,r.JobSeeker.AppUser.Email, r.Title, r.Description, r.Rating)
            ).ToList();
        }

        public async Task RemoveCompany(Guid companyId)
        {
            if (companyId == Guid.Empty)
            {
                throw new ArgumentException("Empty value may not be passed as an argument", nameof(companyId));
            }
            var company = await _companyReadRepository.GetByIdAsync(companyId);
            if (company == null)
            {
                throw new NotFoundException("Company does not exist");
            }
            _companyWriteRepository.Remove(company);
            var recuiter = await _recruiterReadRepository.GetByExpressionAsync(r => r.Company.Id == companyId);
            recuiter.Company = null;
            await _recruiterWriteRepository.SaveChangesAsync();
            await _companyWriteRepository.SaveChangesAsync();
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
            var messagesToDelete = await _context.Messages
                                        .Where(m => m.Sender.Id == appUserId || m.Receiver.Id == appUserId)
                                        .ToListAsync();

            _context.Messages.RemoveRange(messagesToDelete);
            await _context.SaveChangesAsync();

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                throw new Exception("Failed to delete the user");  // Custom Exception
            }
        }

    }
}
