using Bogus;
using CareerCompassAPI.Application.Abstraction.Repositories.ISubscriptionRepository;
using CareerCompassAPI.Domain.Entities;
using CareerCompassAPI.Domain.Enums;
using CareerCompassAPI.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CareerCompassAPI.Persistence.Contexts
{
    public class CareerCompassDbContextInitialiser
    {
        private readonly CareerCompassDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly ISubscriptionReadRepository _subscriptionReadRepository;
        private readonly ISubscriptionWriteRepository _subscriptionWriteRepository;

        public CareerCompassDbContextInitialiser(CareerCompassDbContext context,
                                                 UserManager<AppUser> userManager,
                                                 RoleManager<IdentityRole> roleManager,
                                                 IConfiguration configuration,
                                                 ISubscriptionReadRepository subscriptionReadRepository,
                                                 ISubscriptionWriteRepository   subscriptionWriteRepository)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _subscriptionReadRepository = subscriptionReadRepository;
            _subscriptionWriteRepository = subscriptionWriteRepository;
        }
        public async Task InitialiseAsync()
        {
            await _context.Database.MigrateAsync();
        }
        public async Task RoleSeedAsync()
        {
            foreach (var role in Enum.GetValues(typeof(Roles)))
            {
                if (!await _roleManager.RoleExistsAsync(role.ToString()))
                {
                    await _roleManager.CreateAsync(new() { Name = role.ToString() });
                }
            }
        }
        public async Task SubscriptionsSeedAsync()
        {
            var subscriptions = new List<Subscriptions>
    {
        new Subscriptions { Name = "Free", Price = 0, PostLimit = 1, isPlannerAvailable= false, isChatAvailable = false, isVideoAvailable = false },
        new Subscriptions { Name = "Basic", Price = 149.00M, PostLimit = 3 , isChatAvailable = true, isPlannerAvailable= true, isVideoAvailable = false },
        new Subscriptions { Name = "Pro", Price = 349.00M, PostLimit = 10, isChatAvailable = true, isPlannerAvailable = true, isVideoAvailable = true }
    };
            foreach (var subscription in subscriptions)
            {
                var existingSubscription = await _subscriptionReadRepository.GetByExpressionAsync(s => s.Name == subscription.Name);

                if (existingSubscription is null)
                {
                    await _subscriptionWriteRepository.AddAsync(subscription);
                }
            }
            await _subscriptionWriteRepository.SaveChangesAsync();
        }
        public async Task EducationLevelsSeed()
        {
            var educationLevels = new List<EducationLevel>
            {
             new EducationLevel { Name = "Preschool / Early Childhood Education" },
            new EducationLevel { Name = "Primary / Elementary Education" },
            new EducationLevel { Name = "Middle School / Junior High" },
            new EducationLevel { Name = "High School Diploma / GED" },
            new EducationLevel { Name = "Vocational / Technical Training" },
            new EducationLevel { Name = "Associate's Degree" },
            new EducationLevel { Name = "Bachelor's Degree" },
            new EducationLevel { Name = "Master's Degree" },
            new EducationLevel { Name = "Doctorate / Ph.D." },
            new EducationLevel { Name = "Professional Certification" },
            new EducationLevel { Name = "Some College (no degree)" },
            new EducationLevel { Name = "Other" },
             };
            foreach (var level in educationLevels)
            {
                var existingLevel = await _context.EducationLevels
                    .AnyAsync(el => el.Name == level.Name);
                if (!existingLevel)
                {
                    _context.EducationLevels.Add(level);
                }
            }
            await _context.SaveChangesAsync();
        }
        public async Task UserSeedAsync()
        {
            var proSubId = await _subscriptionReadRepository.GetByExpressionAsync(s => s.PostLimit == -1);
            AppUser appUser = new()
            {
                UserName = _configuration["Master:username"],
                Email = _configuration["Master:email"],
                PhoneNumber = "0502114242"
            };
            await _userManager.CreateAsync(appUser, _configuration["Master:password"]);
            await _userManager.AddToRoleAsync(appUser, Roles.Master.ToString());
        }
        //public async Task RecruiterUserSeedAsync(int count = 20)
        //{
        //    var existingRecruiters = await _userManager.GetUsersInRoleAsync(Roles.Recruiter.ToString());

        //    if (existingRecruiters.Any())
        //    {
        //        return;
        //    }

        //    var faker = new Faker();
        //    var recruiters = new List<AppUser>();

        //    for (int i = 0; i < count; i++)
        //    {
        //        var recruiterUser = new AppUser
        //        {
        //            UserName = faker.Internet.UserName(),
        //            Email = faker.Internet.Email(),
        //            PhoneNumber = faker.Phone.PhoneNumber(),
        //        };
        //        var password = "Rahil123!";

        //        recruiters.Add(recruiterUser);
        //        await _userManager.CreateAsync(recruiterUser, password);
        //        await _userManager.AddToRoleAsync(recruiterUser, Roles.Recruiter.ToString());
        //        await _context.SaveChangesAsync();
        //    }
        //}
        public async Task JobTypeSeed()
        {
            var jobTypes = new List<JobType>()
            {
                new JobType {TypeName = "PartTime"},
                new JobType {TypeName = "FullTime"},
                new JobType {TypeName = "Contract"},
                new JobType {TypeName = "Temporary"},
                new JobType {TypeName = "Internship"},
            };

            foreach (var jobType in jobTypes)
            {
                var existingType = await _context.JobTypes
                    .AnyAsync(j => j.TypeName == jobType.TypeName);

                if (!existingType)
                {
                    _context.JobTypes.Add(jobType);
                }
            }
            await _context.SaveChangesAsync();
        }
        public async Task ExperienceLevelSeed()
        {
            var experienceLevels = new List<ExperienceLevel>
            {
                new ExperienceLevel { LevelName = "Entry"},
                new ExperienceLevel { LevelName = "Mid"},
                new ExperienceLevel { LevelName = "Senior"},
                new ExperienceLevel { LevelName = "No Experience"},
            };

            foreach (var experience in experienceLevels)
            {
                var existingLevel = await _context.ExperienceLevels
                    .AnyAsync(e => e.LevelName == experience.LevelName);
                if (!existingLevel)
                {
                    _context.ExperienceLevels.Add(experience);
                }
            }
            await _context.SaveChangesAsync();
        }
        public async Task ShiftAndScheduleSeed()
        {
            var shifts = new List<ShiftAndSchedule>
            {
                new ShiftAndSchedule {ShiftName = "Day Shift"},
                new ShiftAndSchedule {ShiftName = "Night Shift"},
                new ShiftAndSchedule {ShiftName = "Monday to Friday"},
                new ShiftAndSchedule {ShiftName = "Evening Shift"},
                new ShiftAndSchedule {ShiftName = "Rotating Shift"},
                new ShiftAndSchedule {ShiftName = "No Weekends"},
                new ShiftAndSchedule {ShiftName = "8 Hour Shift"},
                new ShiftAndSchedule {ShiftName = "10 Hour Shift"},
                new ShiftAndSchedule {ShiftName = "12 Hour Shift"},
            };
            foreach (var shift in shifts)
            {
                var existingShift = await _context.ShiftAndSchedules
                    .AnyAsync(s => s.ShiftName == shift.ShiftName);
                if (!existingShift)
                {
                    _context.ShiftAndSchedules.Add(shift);
                }
            }
            await _context.SaveChangesAsync();
        }
        public async Task IndustrySeed()
        {
            var industries = new List<Industry>
            {
                new Industry { Name = "Agriculture"},
                new Industry { Name = "Arts, Entertainment & Recreation"},
                new Industry { Name = "Construction, Repair & Maintenance Services"},
                new Industry { Name = "Education"},
                new Industry { Name = "Energy, Mining & Utilities"},
                new Industry { Name = "Financial Services"},
                new Industry { Name = "Government & Public Administration"},
                new Industry { Name = "Healthcare"},
                new Industry { Name = "Hotels & Travel Accommodation"},
                new Industry { Name = "Human Resources & Staffing"},
                new Industry { Name = "Information Technology"},
                new Industry { Name = "Insurance"},
                new Industry { Name = "Legal"},
                new Industry { Name = "Management & Consulting"},
                new Industry { Name = "Manufacturing"},
                new Industry { Name = "Media & Communication"},
                new Industry { Name = "Nonprofit & NGO"},
                new Industry { Name = "Pharmaceutical & Biotechnology"},
                new Industry { Name = "Real Estate"},
                new Industry { Name = "Restaurants & Food Service"},
                new Industry { Name = "Retail & Wholesale"},
                new Industry { Name = "Telecommunications"},
                new Industry { Name = "Transportation & Logistics"},
            };
            foreach (var industry in industries)
            {
                var existingIndustry = await _context.Industries
                    .AnyAsync(i => i.Name == industry.Name);
                if (!existingIndustry)
                {
                    _context.Industries.Add(industry);
                }
            }
            await _context.SaveChangesAsync();
        }
        public async Task LocationsSeed()
        {
            var locations = new List<JobLocation>
    {
        new JobLocation { Location = "Baku,Azerbaijan" },
        new JobLocation { Location = "Istanbul,Turkey" },
        new JobLocation { Location = "London,UK" },
    };

            foreach (var location in locations)
            {
                var existingLocation = await _context.JobLocations.AnyAsync(j => j.Location.Equals(location.Location));
                if (!existingLocation)
                {
                    _context.JobLocations.Add(location);
                }
            }

            await _context.SaveChangesAsync();
        }
        public async Task SeedJobSeekersAsync()
        {
            Random rand = new Random();
            List<JobSeeker> jobSeekers = new List<JobSeeker>();
            for (int i = 0; i < 20; i++)
            {
                string email = $"user{i}@example.com";
                var user = new AppUser
                {
                    Email = email,
                    UserName = email,
                    DateRegistered = DateTime.Now,
                    PhoneNumber = $"+994501112{i}1",
                };

                var result = await _userManager.CreateAsync(user, "Password@123");

                if (result.Succeeded)
                {
                    var jobSeeker = new JobSeeker
                    {
                        AppUserId = user.Id,
                        AppUser = user,
                        FirstName = $"FirstName{i}",
                        LastName = $"LastName{i}",
                        Location = "Some Location", 
                        IsDeleted = false
                    };

                    jobSeekers.Add(jobSeeker);
                }
            }

            await _context.JobSeekers.AddRangeAsync(jobSeekers);
            await _context.SaveChangesAsync();
        }

        public async Task CompanySeedAsync()
        {
            var techIndustry = await _context.Industries.FirstOrDefaultAsync(i => i.Name == "Information Technology");
            var bakuLocation = await _context.JobLocations.FirstOrDefaultAsync(j => j.Location == "Baku,Azerbaijan");
            var londonLocation = await _context.JobLocations.FirstOrDefaultAsync(j => j.Location == "London,UK");
            var detailsList = new List<CompanyDetails>
    {
        new CompanyDetails
        {
            Ceo = "Sundar Pichai",
            DateFounded = 1998,
            CompanySize = CompanySizeEnum.Size_10000Plus,
            Industry = techIndustry,
            Link = "https://www.google.com",
            Description = "Google is a tech company specializing in Internet-related services and products.",
            Address = "1600 Amphitheatre Parkway, Mountain View, CA",
            Location = londonLocation
        },
        new CompanyDetails
        {
            Ceo = "Satya Nadella",
            DateFounded = 1975,
            CompanySize = CompanySizeEnum.Size_10000Plus,
            Industry = techIndustry,
            Link = "https://www.microsoft.com",
            Description = "Microsoft develops, licenses, and supports a wide range of software products, computing devices, and services.",
            Address = "Redmond, Washington, U.S.",
            Location = bakuLocation
        },
        new CompanyDetails
        {
            Ceo = "Tim Cook",
            DateFounded = 1976,
            CompanySize = CompanySizeEnum.Size_10000Plus,
            Industry = techIndustry,
            Link = "https://www.apple.com",
            Description = "Apple designs, manufactures, and markets mobile communication and media devices.",
            Address = "Cupertino, California, U.S.",
            Location = londonLocation
        }
    };

            await _context.CompanyDetails.AddRangeAsync(detailsList);
            await _context.SaveChangesAsync();
            var companies = new List<Company>
    {
        new Company { Name = "Google", Details = detailsList[0] },
        new Company { Name = "Microsoft", Details = detailsList[1] },
        new Company { Name = "Apple", Details = detailsList[2] }
    };

            foreach (var company in companies)
            {
                var existingCompany = await _context.Companies.FirstOrDefaultAsync(c => c.Name == company.Name);
                if (existingCompany == null)
                {
                    await _context.Companies.AddAsync(company);
                }
            }

            await _context.SaveChangesAsync();
        }
        public async Task SeedReviewsAsync()
        {
            List<Review> reviews = new List<Review>();
            Random rand = new Random();

            string[] companies = { "Apple", "Microsoft", "Google" };
            var jobSeekerIds = await _context.JobSeekers
                                .Select(js => js.Id)
                                .ToListAsync();

            foreach (var companyName in companies)
            {
                var company = await _context.Companies
                    .FirstOrDefaultAsync(c => c.Name == companyName);

                if (company == null) continue;

                foreach (var jobSeekerId in jobSeekerIds)
                {
                    var jobSeeker = await _context.JobSeekers
                        .FirstOrDefaultAsync(js => js.Id == jobSeekerId);

                    if (jobSeeker == null) continue;

                    var review = new Review
                    {
                        JobSeeker = jobSeeker,
                        Title = $"Review for {companyName}",
                        Description = $"This is a review for {companyName} by job seeker {jobSeeker.Id}.",
                        Rating = rand.Next(1, 6), 
                        Company = company,
                        Status = ReviewStatus.Approved
                    };

                    reviews.Add(review);
                }
            }

            await _context.Reviews.AddRangeAsync(reviews);
            await _context.SaveChangesAsync();
        }




        public async Task SettingsSeed()
        {
            var defaultSettings = new List<AppSetting>
            {
                new AppSetting { SettingName = "DaysToDeleteOldMessages", SettingValue = "3"},
                new AppSetting {SettingName = "DaysToDeleteOldNotifications", SettingValue ="3"},
                new AppSetting {SettingName = "DaysToDeleteDeclinedApplications", SettingValue="7"},
                new AppSetting {SettingName = "DaysToDeleteDeclinedReviews", SettingValue="3"},
             };

            foreach (var defaultSetting in defaultSettings)
            {
                var existingSetting = await _context.Settings
                    .Where(s => s.SettingName == defaultSetting.SettingName)
                    .FirstOrDefaultAsync();

                if (existingSetting == null)
                {
                    await _context.Settings.AddAsync(defaultSetting);
                }
            }

            await _context.SaveChangesAsync();
        }

    }
}
