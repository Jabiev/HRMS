using Bogus;
using CareerCompassAPI.Application.Abstraction.Repositories.ISubscriptionRepository;
using CareerCompassAPI.Domain.Entities;
using CareerCompassAPI.Domain.Enums;
using CareerCompassAPI.Domain.Identity;
using Hangfire.Dashboard;
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
                                                 ISubscriptionWriteRepository subscriptionWriteRepository)
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
        new Subscriptions { Name = "Free", Price = 0, PostLimit = 1 },
        new Subscriptions { Name = "Basic", Price = 149.00M, PostLimit = 3 },
        new Subscriptions { Name = "Pro", Price = 349.00M, PostLimit = 10 }
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
        public async Task RecruiterUserSeedAsync(int count = 20)
        {
            var existingRecruiters = await _userManager.GetUsersInRoleAsync(Roles.Recruiter.ToString());

            if (existingRecruiters.Any())
            {
                return;
            }

            var faker = new Faker();
            var recruiters = new List<AppUser>();

            for (int i = 0; i < count; i++)
            {
                var recruiterUser = new AppUser
                {
                    UserName = faker.Internet.UserName(),
                    Email = faker.Internet.Email(),
                    PhoneNumber = faker.Phone.PhoneNumber(),
                };
                var password = "Rahil123!";

                recruiters.Add(recruiterUser);
                await _userManager.CreateAsync(recruiterUser, password);
                await _userManager.AddToRoleAsync(recruiterUser, Roles.Recruiter.ToString());
                await _context.SaveChangesAsync();
            }
        }

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

    }
}
