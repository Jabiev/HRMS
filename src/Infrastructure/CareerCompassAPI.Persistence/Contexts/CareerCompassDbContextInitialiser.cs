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
        new Subscriptions { Name = "Free", Price = 0, PostLimit = 3 },
        new Subscriptions { Name = "Basic", Price = 29.99M, PostLimit = 10 },
        new Subscriptions { Name = "Pro", Price = 59.99M, PostLimit = -1 }
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

        public async Task UserSeedAsync()
        {
            var proSubId = await _subscriptionReadRepository.GetByExpressionAsync(s => s.PostLimit == -1);
            AppUser appUser = new()
            {
                UserName = _configuration["Master:username"],
                Email = _configuration["Master:email"],
            };
            await _userManager.CreateAsync(appUser, _configuration["Master:password"]);
            await _userManager.AddToRoleAsync(appUser, Roles.Master.ToString());
        }
        public async Task JobTypeSeed()
        {
            var jobTypes = new List<JobType>()
            {
                new JobType {TypeName = "PartTime"},
                new JobType {TypeName = "FullTime"},
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

    }
}
