using CareerCompassAPI.Application.Abstraction.Repositories.ICompanyRepositories;
using CareerCompassAPI.Application.Abstraction.Repositories.IRecruiterRepositories;
using CareerCompassAPI.Application.Abstraction.Repositories.IReviewRepositories;
using CareerCompassAPI.Application.Abstraction.Repositories.ITeamRepositories;
using CareerCompassAPI.Application.Abstraction.Services;
using CareerCompassAPI.Application.DTOs.AppSetting_DTOs;
using CareerCompassAPI.Application.DTOs.AppUser_DTOs;
using CareerCompassAPI.Application.DTOs.Dashboard_DTOs;
using CareerCompassAPI.Application.DTOs.ExperienceLevel_DTOs;
using CareerCompassAPI.Application.DTOs.JobType_DTOs;
using CareerCompassAPI.Application.DTOs.Location_DTOs;
using CareerCompassAPI.Application.DTOs.Resume_DTOs;
using CareerCompassAPI.Application.DTOs.Schedule_DTOs;
using CareerCompassAPI.Application.DTOs.TeamMember_DTOs;
using CareerCompassAPI.Domain.Concretes;
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
        private readonly ITeamWriteRepository _teamWriteRepository;


        public DashboardService(UserManager<AppUser> userManager,
                                CareerCompassDbContext context,
                                ICompanyReadRepository companyReadRepository,
                                ICompanyWriteRepository companyWriteRepository,
                                IRecruiterReadRepository recruiterReadRepository,
                                IRecruiterWriteRepository recruiterWriteRepository,
                                IReviewReadRepository reviewReadRepository,
                                IReviewWriteRepository reviewWriteRepository,
                                ITeamWriteRepository teamWriteRepository)
        {
            _userManager = userManager;
            _context = context;
            _companyReadRepository = companyReadRepository;
            _companyWriteRepository = companyWriteRepository;
            _recruiterReadRepository = recruiterReadRepository;
            _recruiterWriteRepository = recruiterWriteRepository;
            _reviewReadRepository = reviewReadRepository;
            _reviewWriteRepository = reviewWriteRepository;
            _teamWriteRepository = teamWriteRepository;
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

        public async Task<Guid> CreateJobType(JobTypeCreateDto jobTypeCreateDto)
        {
            if (jobTypeCreateDto is null)
            {
                throw new ArgumentNullException("Passed in parameter may not contain null values");
            }
            JobType newJobType = new()
            {
                TypeName = jobTypeCreateDto.typeName
            };
            await _context.AddAsync(newJobType);
            await _context.SaveChangesAsync();
            return newJobType.Id;
        }

        public async Task<Guid> CreateShift(ShiftAndScheduleCreateDto shiftAndScheduleDto)
        {
            if (shiftAndScheduleDto is null)
            {
                throw new ArgumentNullException("Passed in argument may not contain null values");
            }
            ShiftAndSchedule newShiftAndSchedule = new()
            {
                ShiftName = shiftAndScheduleDto.shiftName
            };
            await _context.AddAsync(newShiftAndSchedule);
            await _context.SaveChangesAsync();
            return newShiftAndSchedule.Id;
        }

        public async Task<Guid> CreateSubscription(Application.DTOs.Subscription_DTOs.SubscriptionCreateDto subscriptionCreateDto)
        {
            if (subscriptionCreateDto is null)
            {
                throw new ArgumentNullException("Parameters passed in may not include null values");
            }
            Subscriptions newSubscription = new()
            {
                Name = subscriptionCreateDto.Name,
                Price = subscriptionCreateDto.Price,
                PostLimit = subscriptionCreateDto.PostLimit
            };
            await _context.Subscriptions.AddAsync(newSubscription);
            await _context.SaveChangesAsync();
            return newSubscription.Id;
        }

        public async Task<Guid> CreateMember(TeamMemberCreateDto teamMemberCreateDto)
        {
            if (teamMemberCreateDto is null)
            {
                throw new ArgumentNullException("Parameter passed in may not include null values");
            }
            TeamMember newMember = new()
            {
                FirstName = teamMemberCreateDto.firstName,
                LastName = teamMemberCreateDto.lastName,
                Description = teamMemberCreateDto.description,
                Position = teamMemberCreateDto.position,
                ImageUrl = teamMemberCreateDto.imageUrl,
            };
            await _teamWriteRepository.AddAsync(newMember);
            await _teamWriteRepository.SaveChangesAsync();
            return newMember.Id;
        }
        public async Task<PaginatedResponse<AppUserGetDto>> GetAllAsync(string searchQuery = "", int pageNumber = 1, int pageSize = 10)
        {
            var appUsers = new List<AppUserGetDto>();
            IQueryable<AppUser> queryableUsers = _userManager.Users;

            if (!string.IsNullOrEmpty(searchQuery))
            {
                queryableUsers = queryableUsers.Where(
                    u => u.Id.Contains(searchQuery) ||
                    u.UserName.ToLower().Contains(searchQuery.ToLower()) ||
                    u.Email.ToLower().Contains(searchQuery.ToLower()) ||
                    u.PhoneNumber.ToLower().Contains(searchQuery.ToLower())
                );
            }
            int totalItems = queryableUsers.Count();
            var users = await queryableUsers
                                .Skip((pageNumber - 1) * pageSize)
                                .Take(pageSize)
                                .ToListAsync();

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

            return new PaginatedResponse<AppUserGetDto>(appUsers, totalItems);
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

        public async Task<PaginatedResponse<UserFeedbacksGetDto>> GetAllFeedbacksAsync(int page, int pageSize)
        {
            var totalItems = await _context.TestimonialFeedbacks.CountAsync();

            var feedbackList = await _context.TestimonialFeedbacks
                                              .Skip((page - 1) * pageSize)
                                              .Take(pageSize)
                                              .ToListAsync();

            if (feedbackList.Count == 0)
            {
                throw new NotFoundException("No Feedbacks found");
            }

            List<UserFeedbacksGetDto> feedbacks = feedbackList.Select(feedback => new UserFeedbacksGetDto(
                feedback.Id,
                feedback.FirstName,
                feedback.LastName,
                feedback.Description,
                feedback.ImageUrl,
                feedback.JobTitle,
                feedback.isActive
            )).ToList();

            return new PaginatedResponse<UserFeedbacksGetDto>(feedbacks, totalItems);
        }

        public async Task<List<LocationGetDto>> GetAllLocationsAsync(string? searchQuery)
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

        public async Task<PaginatedResponse<PaymentsListGetDto>> GetAllPaymentsAsync(int page, int pageSize)
        {
            var totalItems = await _context.Payments.CountAsync();
            var paymentsList = await _context.Payments
                                             .Include(p => p.AppUser)
                                             .Skip((page - 1) * pageSize)
                                             .Take(pageSize)
                                             .ToListAsync();

            if (paymentsList.Count == 0)
            {
                throw new NotFoundException("No Payments found");
            }

            List<PaymentsListGetDto> payments = paymentsList.Select(payment => new PaymentsListGetDto(
                payment.Id,
                payment.AppUser.Id,
                payment.Amount,
                Enum.GetName(typeof(PaymentTypes), payment.Type),
                payment.DateCreated.ToString("yyyy-MM-ddTHH:mm:ss")
            )).ToList();

            return new PaginatedResponse<PaymentsListGetDto>(payments, totalItems);
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
                new PendingReviewsDto(r.Id, r.JobSeeker.AppUser.Email, r.Title, r.Description, r.Rating)
            ).ToList();
        }

        public async Task<List<ShiftAndScheduleGetDto>> GetAllShiftsAsync()
        {
            var shifts = await _context.ShiftAndSchedules.ToListAsync();
            if (shifts.Count == 0)
            {
                throw new NotFoundException("Shifts not found");
            }
            List<ShiftAndScheduleGetDto> shiftAndSchedules = shifts.Select(shift => new ShiftAndScheduleGetDto(shift.Id, shift.ShiftName)).ToList();
            return shiftAndSchedules;
        }

        public async Task<List<SubscriptionGetDto>> GetAllSubscriptionsAsync()
        {
            var subscriptions = await _context.Subscriptions.ToListAsync();
            if (subscriptions.Count == 0)
            {
                throw new NotFoundException("Subscriptions not found");
            }
            List<SubscriptionGetDto> subs = subscriptions.Select(sub => new SubscriptionGetDto(sub.Id, sub.Name, sub.Price, sub.PostLimit)).ToList();
            return subs;
        }

        public async Task<List<JobTypeGetDto>> GetAllTypesAsync()
        {
            var jobTypes = await _context.JobTypes.ToListAsync();
            if (jobTypes.Count == 0)
            {
                throw new NotFoundException("Job types do not exist");
            }
            List<JobTypeGetDto> types = jobTypes.Select(type => new JobTypeGetDto(type.Id, type.TypeName)).ToList();
            return types;
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

        public async Task<List<TeamMembersGetDto>> GetAllTeamMembers()
        {
            var teamMembers = await _context.Members.Where(m => m.IsDeleted == false).ToListAsync();
            if (teamMembers.Count == 0)
            {
                throw new NotFoundException("Team Members do not exist");
            }
            List<TeamMembersGetDto> members = teamMembers.Select(member => new TeamMembersGetDto(member.Id, member.FirstName, member.LastName,member.Position, member.Description, member.ImageUrl)).ToList();
            return members;
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

        public async Task RemoveFeedback(Guid feedbackId)
        {
            if (feedbackId == Guid.Empty)
            {
                throw new NotFoundException("Feedback does not exist");
            }
            TestimonialFeedback feedback = await _context.TestimonialFeedbacks.FirstOrDefaultAsync(tf => tf.Id ==  feedbackId);
            _context.Remove(feedback);
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

        public async Task RemoveJobType(Guid jobTypeId)
        {
            if (jobTypeId == Guid.Empty)
            {
                throw new NotFoundException("Job type with given ID does not exist");
            }
            JobType jobType = await _context.JobTypes.FirstOrDefaultAsync(jt => jt.Id == jobTypeId);
            _context.Remove(jobType);
            _context.SaveChangesAsync();
        }

        public async Task RemoveShift(Guid shiftId)
        {
            if (shiftId == Guid.Empty)
            {
                throw new ArgumentNullException("Empty value may not be passed as an argument");
            }
            ShiftAndSchedule shift = await _context.ShiftAndSchedules.FirstOrDefaultAsync(sh => sh.Id == shiftId);
            _context.Remove(shift);
            _context.SaveChangesAsync();
        }

        public async Task RemoveSubscription(Guid subscriptionId)
        {
            if (subscriptionId == Guid.Empty)
            {
                throw new NotFoundException("Subscription with given ID does not exist");
            }
            Subscriptions subscriptions = await _context.Subscriptions.FirstOrDefaultAsync(s => s.Id == subscriptionId);
            _context.Remove(subscriptions);
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

        public async Task SetIsActive(Guid feedbackId)
        {
            if (feedbackId == Guid.Empty)
            {
                throw new NotFoundException("Feedback does not exist");
            }
            TestimonialFeedback feedback = await _context.TestimonialFeedbacks.FirstOrDefaultAsync(tf => tf.Id ==  feedbackId);
            feedback.isActive = true;
            _context.SaveChangesAsync();
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
            review.Status = newStatus;
            _reviewWriteRepository.Update(review);
            await _reviewWriteRepository.SaveChangesAsync();
        }

        public async Task UpdateSubscription(SubscriptionUpdateDto updateSubscriptionDto)
        {
            var subscription = await _context.Subscriptions.FindAsync(updateSubscriptionDto.id);
            if (subscription == null)
            {
                throw new NotFoundException("Subscription with given ID is not found");
            }
            subscription.Name = updateSubscriptionDto.name;
            subscription.Price = updateSubscriptionDto.price;
            subscription.PostLimit = updateSubscriptionDto.postLimit;
            _context.Subscriptions.Update(subscription);
            await _context.SaveChangesAsync();
        }

        public async Task<List<AppSettingGetDto>> GetAppSettingListAsync()
        {
            var settingsList = await _context.Settings.ToListAsync();
            if (settingsList.Count == 0)
            {
                throw new NotFoundException("Settings do not exist");
            }

            List<AppSettingGetDto> settings = settingsList.Select(
                setting => new AppSettingGetDto(setting.Id,setting.SettingName, setting.SettingValue)
            ).ToList();

            return settings;
        }

        public async Task<List<ResumeGetDto>> GetAllResumes()
        {
            var resumeList = await _context.Resumes.ToListAsync();
            if (resumeList.Count == 0)
            {
                throw new NotFoundException("Resumes do not exist");
            }
            List<ResumeGetDto> resumes = resumeList.Select(resume => new ResumeGetDto(resume.Id, resume.Name, resume.Price, resume.Description, resume.Structure)).ToList();    
            return resumes;
        }

        public async Task RemoveResume(Guid resumeId)
        {
            if (resumeId == Guid.Empty)
            {
                throw new ArgumentNullException("Argument passed in may not contain null values");
            }
            Resume resume = await _context.Resumes.FirstOrDefaultAsync(r => r.Id == resumeId);
            _context.Remove(resume);
            _context.SaveChangesAsync();
        }
    }
}
