using CareerCompassAPI.Application.Abstraction.Repositories.ICompanyRepositories;
using CareerCompassAPI.Application.Abstraction.Repositories.IRecruiterRepositories;
using CareerCompassAPI.Application.Abstraction.Repositories.IReviewRepositories;
using CareerCompassAPI.Application.Abstraction.Services;
using CareerCompassAPI.Application.DTOs.AppUser_DTOs;
using CareerCompassAPI.Application.DTOs.Dashboard_DTOs;
using CareerCompassAPI.Application.DTOs.ExperienceLevel_DTOs;
using CareerCompassAPI.Application.DTOs.Location_DTOs;
using CareerCompassAPI.Domain.Entities;
using CareerCompassAPI.Domain.Enums;
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
        private readonly ICompanyReadRepository _companyReadRepository;
        private readonly ICompanyWriteRepository _companyWriteRepository;
        private readonly IRecruiterReadRepository _recruiterReadRepository;
        private readonly IRecruiterWriteRepository _recruiterWriteRepository;
        private readonly IReviewReadRepository _reviewReadRepository;
        private readonly IReviewWriteRepository _reviewWriteRepository;
        

        public DashboardService(UserManager<AppUser> userManager,
                                CareerCompassDbContext context,
                                ICompanyReadRepository companyReadRepository,
                                ICompanyWriteRepository companyWriteRepository,
                                IRecruiterReadRepository recruiterReadRepository,
                                IRecruiterWriteRepository recruiterWriteRepository,
                                IReviewReadRepository reviewReadRepository,
                                IReviewWriteRepository reviewWriteRepository)
        {
            _userManager = userManager;
            _context = context;
            _companyReadRepository = companyReadRepository;
            _companyWriteRepository = companyWriteRepository;
            _recruiterReadRepository = recruiterReadRepository;
            _recruiterWriteRepository = recruiterWriteRepository;
            _reviewReadRepository = reviewReadRepository;
            _reviewWriteRepository = reviewWriteRepository;
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

        public async Task<Guid> CreateEducationLevel(CreateEducationLevelDto createEducationLevelDto)
        {
            if (createEducationLevelDto is null)
            {
                throw new ArgumentNullException("Arguments passed in may not contain null values");
            }
            EducationLevel newLevel = new()
            {
                Name = createEducationLevelDto.name
            };
            await _context.EducationLevels.AddAsync(newLevel);
            await _context.SaveChangesAsync();
            return newLevel.Id;
        }

        public async Task<Guid> CreateExperienceLevel(ExperienceLevelCreateDto experienceLevelCreateDto)
        {
            if (experienceLevelCreateDto is null)
            {
                throw new ArgumentNullException("Null values may not be passed as an argument");
            }
            ExperienceLevel newLevel = new()
            {
                LevelName = experienceLevelCreateDto.name
            };
            await _context.AddAsync(newLevel);
            await _context.SaveChangesAsync();
            return newLevel.Id;
        }

        public async Task<Guid> CreateJobLocation(JobLocationCreateDto jobLocationCreateDto)
        {
            if (jobLocationCreateDto is null)
            {
                throw new ArgumentNullException("Parameter passed in may not contain null values");
            }
            JobLocation newLocation = new()
            {
                Location = jobLocationCreateDto.locationName
            };
            await _context.AddAsync(newLocation);
            await _context.SaveChangesAsync();
            return newLocation.Id;
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
                 .Where(c => c.IsDeleted == false)
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

        public async Task<List<EducationLevelsGetDto>> GetAllEducationLevelsAsync()
        {
            var levels = await _context.EducationLevels.ToListAsync();
            if (levels.Count == 0)
            {
                throw new NotFoundException("Education levels do not exist");
            }

            List<EducationLevelsGetDto> newLevels = levels.Select(level =>
                new EducationLevelsGetDto(level.Id, level.Name)
            ).ToList();
            return newLevels;
        }

        public async Task<List<ExperienceLevelGetDto>> GetAllExperienceLevelsAsync()
        {
            var experienceLevels = await _context.ExperienceLevels.ToListAsync();
            if (experienceLevels.Count == 0)
            {
                throw new NotFoundException("Experience levels do not exist");
            }
            List<ExperienceLevelGetDto> expLevels = experienceLevels.Select(level =>
            new ExperienceLevelGetDto(level.Id, level.LevelName)).ToList();
            return expLevels;
        }
        public async Task<List<LocationGetDto>> GetAllLocationsAsync(string searchQuery = " ")
        {
            IQueryable<JobLocation> query = _context.JobLocations;

            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                query = query.Where(location => location.Location.Contains(searchQuery));
            }

            var jobLocations = await query.ToListAsync();

            if (jobLocations.Count == 0)
            {
                throw new NotFoundException("Job locations do not exist");
            }

            List<LocationGetDto> locations = jobLocations.Select(location => new LocationGetDto(location.Id, location.Location)).ToList();

            return locations;
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

        public async Task<List<UserRegistrationStatDto>> GetUserRegistrationStatsAsync(DateTime? startDate, DateTime? endDate)
        {
            var query = _context.Users.Include(u => u.JobSeekers)
                    .Include(u => u.Recruiters)
                    .AsQueryable(); 

            if (startDate.HasValue)
            {
                query = query.Where(u => u.DateRegistered >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                query = query.Where(u => u.DateRegistered <= endDate.Value);
            }

            var users = await query.ToListAsync();
            return users
                .GroupBy(u => u.DateRegistered.Date)
                .Select(g => new UserRegistrationStatDto(
                Date: g.Key,
                TotalUsers: g.Count(),
                JobSeekers: g.Count(u => u.JobSeekers != null),
                Recruiters: g.Count(u => u.Recruiters != null)
                ))
                .OrderBy(x => x.Date)
                .ToList();

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
            company.IsDeleted = true;
            _companyWriteRepository.Update(company);
            var recuiter = await _recruiterReadRepository.GetByExpressionAsync(r => r.Company.Id == companyId);
            recuiter.Company = null;
            await _recruiterWriteRepository.SaveChangesAsync();
            await _companyWriteRepository.SaveChangesAsync();
        }

        public async Task RemoveEducationLevel(Guid levelId)
        {
            if (levelId == Guid.Empty)
            {
                throw new NotFoundException("Parameter passed may not contain null value");
            }
            EducationLevel level = await _context.EducationLevels.FirstOrDefaultAsync(el => el.Id == levelId);
            _context.Remove(level);
            _context.SaveChangesAsync();
        }

        public async Task RemoveExperienceLevel(Guid levelId)
        {
            if (levelId == Guid.Empty)
            {
                throw new NotFoundException("Parameter passed may not contain null value");
            }
            ExperienceLevel level = await _context.ExperienceLevels.FirstOrDefaultAsync(el => el.Id == levelId);
            _context.Remove(level);
            _context.SaveChangesAsync();
        }

        public async Task RemoveJobLocation(Guid jobLocationId)
        {
            if (jobLocationId == Guid.Empty)
            {
                throw new NotFoundException("Location does not exist with given parameter");
            }
            JobLocation location = await _context.JobLocations.FirstOrDefaultAsync(jl => jl.Id == jobLocationId);
            _context.Remove(location);
            _context.SaveChangesAsync();
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

        public async Task UpdateEducationLevel(EducationLevelUpdateDto updateEducationLevelDto)
        {
            if (updateEducationLevelDto is null)
            {
                throw new NotFoundException("Argument passed in may not contain null values");
            }
            EducationLevel level = _context.EducationLevels.FirstOrDefault(el => el.Id == updateEducationLevelDto.levelId);
            if (level is null)
            {
                throw new NotFoundException("Level with given ID does not exist");
            }
            level.Name = updateEducationLevelDto.newName;
            _context.EducationLevels.Update(level);
           await _context.SaveChangesAsync();
        }

        public async Task UpdateReviewStatus(Guid reviewId, ReviewStatus newStatus)
        {
            var review = await _reviewReadRepository.GetByIdAsync(reviewId);
            if (review is null)
            {
                throw new NotFoundException("Review not found"); 
            }
            review.Status= newStatus;
            _reviewWriteRepository.Update(review);
            await _reviewWriteRepository.SaveChangesAsync();
        }
    }
}
