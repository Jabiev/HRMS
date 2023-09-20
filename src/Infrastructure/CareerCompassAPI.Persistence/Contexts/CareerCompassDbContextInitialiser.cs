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
        public async Task SeedRecruitersAsync()
        {
            Random rand = new Random();
            List<Recruiter> recruiters = new List<Recruiter>();

            var freeSubscription = await _context.Subscriptions.FirstOrDefaultAsync(s => s.Name == "Free");
            var basicSubscription = await _context.Subscriptions.FirstOrDefaultAsync(s => s.Name == "Basic");
            var proSubscription = await _context.Subscriptions.FirstOrDefaultAsync(s => s.Name == "Pro");
            Subscriptions[] possibleSubscriptions = { freeSubscription, basicSubscription, proSubscription };

            var companies = await _context.Companies.ToListAsync();

            for (int i = 0; i < 20; i++)
            {
                string email = $"recruiter{i}@example.com";
                var user = new AppUser
                {
                    Email = email,
                    UserName = email,
                    DateRegistered = DateTime.Now,
                    PhoneNumber = $"+994501112{i}2",
                };

                var result = await _userManager.CreateAsync(user, "Password@123");

                if (result.Succeeded)
                {
                    Console.WriteLine($"User created with ID: {user.Id}");

                    var subscription = possibleSubscriptions[rand.Next(0, possibleSubscriptions.Length)];
                    var company = companies[rand.Next(0, companies.Count)];

                    var recruiter = new Recruiter
                    {
                        AppUserId = user.Id,
                        AppUser = user,
                        CompanyId = company.Id,
                        Company = company,
                        FirstName = $"RecruiterFirstName{i}",
                        LastName = $"RecruiterLastName{i}",
                        Subscription = subscription,
                        CurrentPostCount = 0,
                        SubscriptionStartDate = DateTime.Now
                    };

                    recruiters.Add(recruiter);
                }
            }

            if (recruiters.Any())
            {
                await _context.Recruiters.AddRangeAsync(recruiters);
                await _context.SaveChangesAsync();
            }
        }
        public async Task SeedVacanciesAsync()
        {
            Random rand = new Random();

            var recruiters = await _context.Recruiters.ToListAsync();
            var entryLevel = await _context.ExperienceLevels.FirstOrDefaultAsync(e => e.LevelName == "Entry");
            var midLevel = await _context.ExperienceLevels.FirstOrDefaultAsync(e => e.LevelName == "Mid");
            var seniorLevel = await _context.ExperienceLevels.FirstOrDefaultAsync(e => e.LevelName == "Senior");
            var noExperience = await _context.ExperienceLevels.FirstOrDefaultAsync(e => e.LevelName == "No Experience");
            var london = await _context.JobLocations.FirstOrDefaultAsync(l => l.Location == "London,UK");
            var baku = await _context.JobLocations.FirstOrDefaultAsync(l => l.Location == "Baku,Azerbaijan");
            var istanbul = await _context.JobLocations.FirstOrDefaultAsync(l => l.Location == "Istanbul,Turkey");
            var google = await _context.Companies.FirstOrDefaultAsync(c => c.Name == "Google");
            var jpmorgan = await _context.Companies.FirstOrDefaultAsync(c => c.Name == "JP Morgan");
            List<Vacancy> vacancies = new List<Vacancy>();
            vacancies.Add(new Vacancy
            {
                JobTitle = "Finance Manager",
                ExperienceLevel = midLevel,
                Recruiter = recruiters[rand.Next(recruiters.Count)],
                Salary = 80000.00M,
                JobLocation = london,
                Description = "Responsible for managing the financial aspects of our London branch.",
                Company = jpmorgan,
                ApplicationLimit = 50,
                CurrentApplicationCount = 0
            });

            vacancies.Add(new Vacancy
            {
                JobTitle = "Software Engineer",
                ExperienceLevel = entryLevel,
                Recruiter = recruiters[rand.Next(recruiters.Count)],
                Salary = 100000.00M,
                JobLocation = baku,
                Description = "Software Engineer position in the Baku office focusing on web technologies.",
                Company = google,
                ApplicationLimit = 100,
                CurrentApplicationCount = 0
            });


            await _context.Vacancy.AddRangeAsync(vacancies);
            await _context.SaveChangesAsync();
        }

        public async Task CompanySeedAsync()
        {
            if (!_context.CompanyDetails.Any())
            {
                var techIndustry = await _context.Industries.FirstOrDefaultAsync(i => i.Name == "Information Technology");
                var foodIndustry = await _context.Industries.FirstOrDefaultAsync(i => i.Name == "Restaurants & Food Service");
                var transportationIndustry = await _context.Industries.FirstOrDefaultAsync(i => i.Name == "Transportation & Logistics");
                var agricultureIndustry = await _context.Industries.FirstOrDefaultAsync(i => i.Name == "Agriculture");
                var financialIndustry = await _context.Industries.FirstOrDefaultAsync(i => i.Name == "Financial Services");
                var healthcareIndustry = await _context.Industries.FirstOrDefaultAsync(i => i.Name == "Healthcare");
                var bakuLocation = await _context.JobLocations.FirstOrDefaultAsync(j => j.Location == "Baku,Azerbaijan");
                var londonLocation = await _context.JobLocations.FirstOrDefaultAsync(j => j.Location == "London,UK");
                var companies = new List<Company>
        {
            new Company
            {
                Name = "Google",
                Details = new CompanyDetails
                {
                    Ceo = "Sundar Pichai",
                    DateFounded = 1998,
                    CompanySize = CompanySizeEnum.Size_10000Plus,
                    Industry = techIndustry,
                    Link = "https://www.google.com",
                    Description = "Google is a tech company specializing in Internet-related services and products.",
                    Address = "1600 Amphitheatre Parkway, Mountain View, CA",
                    Location = londonLocation
                }
            },
            new Company
            {
                Name = "FedEx",
                Details = new CompanyDetails
                {
                    Ceo = "Frederick W. Smith",
                    DateFounded = 1971,
                    CompanySize = CompanySizeEnum.Size_10000Plus,
                    Industry = transportationIndustry,
                    Link = "https://www.fedex.com",
                    Description = "An American multinational delivery services company.",
                    Address = "Memphis, Tennessee, U.S.",
                    Location = bakuLocation
                }
            },
            new Company
            {
                Name = "Marriott International",
                Details = new CompanyDetails
                {
                     Ceo = "Anthony Capuano",
                     DateFounded = 1927,
                     CompanySize = CompanySizeEnum.Size_10000Plus,
                     Industry = await _context.Industries.FirstOrDefaultAsync(i => i.Name == "Hotels & Travel Accommodation"),
                      Link = "https://www.marriott.com",
                      Description = "Marriott International is an American multinational company that operates, franchises, and licenses lodging including hotel, residential, and timeshare properties.",
                      Address = "Bethesda, Maryland, U.S.",
                      Location = await _context.JobLocations.FirstOrDefaultAsync(j => j.Location == "London,UK")
                }
            },
            new Company
{
    Name = "State Farm",
    Details = new CompanyDetails
    {
        Ceo = "Michael L. Tipsord",
        DateFounded = 1922,
        CompanySize = CompanySizeEnum.Size_10000Plus,
        Industry = await _context.Industries.FirstOrDefaultAsync(i => i.Name == "Insurance"),
        Link = "https://www.statefarm.com",
        Description = "State Farm is a large group of insurance and financial services companies.",
        Address = "Bloomington, Illinois, U.S.",
        Location = londonLocation
    }
},
new Company
{
    Name = "Rockstar Games",
    Details = new CompanyDetails
    {
        Ceo = "Strauss Zelnick",
        DateFounded = 1998,
        CompanySize = CompanySizeEnum.Size_2501_5000,
        Industry = await _context.Industries.FirstOrDefaultAsync(i => i.Name == "Arts, Entertainment & Recreation"),
        Link = "https://www.rockstargames.com",
        Description = "Rockstar Games is an American video game publisher known for franchises like Grand Theft Auto and Red Dead Redemption.",
        Address = "New York, NY, USA",
        Location = bakuLocation 
    }
},
new Company
{
    Name = "Ubisoft",
    Details = new CompanyDetails
    {
        Ceo = "Yves Guillemot",
        DateFounded = 1986,
        CompanySize = CompanySizeEnum.Size_10000Plus,
        Industry = await _context.Industries.FirstOrDefaultAsync(i => i.Name == "Arts, Entertainment & Recreation"),
        Link = "https://www.ubisoft.com",
        Description = "Ubisoft is a French video game company known for titles like Assassin's Creed and Rainbow Six.",
        Address = "Montreuil, France",
        Location = londonLocation 
    }
},
new Company
{
    Name = "CD Projekt",
    Details = new CompanyDetails
    {
        Ceo = "Adam Kiciński",
        DateFounded = 1994,
        CompanySize = CompanySizeEnum.Size_2501_5000,
        Industry = await _context.Industries.FirstOrDefaultAsync(i => i.Name == "Arts, Entertainment & Recreation"),
        Link = "https://www.cdprojekt.com",
        Description = "CD Projekt is a Polish video game company famous for the Witcher series and Cyberpunk 2077.",
        Address = "Warsaw, Poland",
        Location = bakuLocation
    }
},

new Company
{
    Name = "Icherisheher Art Gallery",
    Details = new CompanyDetails
    {
        Ceo = "TBD",
        DateFounded = 2001,
        CompanySize = CompanySizeEnum.Size_1_50,
        Industry = await _context.Industries.FirstOrDefaultAsync(i => i.Name == "Arts, Entertainment & Recreation"),
        Link = "http://example.com",
        Description = "An art gallery located in the historic Icherisheher.",
        Address = "Icherisheher, Baku, Azerbaijan",
        Location = bakuLocation
    }
},
new Company
{
    Name = "Balfour Beatty",
    Details = new CompanyDetails
    {
        Ceo = "Leo Quinn",
        DateFounded = 1909,
        CompanySize = CompanySizeEnum.Size_10000Plus,
        Industry = await _context.Industries.FirstOrDefaultAsync(i => i.Name == "Construction, Repair & Maintenance Services"),
        Link = "https://www.balfourbeatty.com",
        Description = "Balfour Beatty is an infrastructure group.",
        Address = "London, UK",
        Location = londonLocation
    }
},

new Company
{
    Name = "BP",
    Details = new CompanyDetails
    {
        Ceo = "Bernard Looney",
        DateFounded = 1909,
        CompanySize = CompanySizeEnum.Size_10000Plus,
        Industry = await _context.Industries.FirstOrDefaultAsync(i => i.Name == "Energy, Mining & Utilities"),
        Link = "https://www.bp.com",
        Description = "BP is a British multinational oil and gas company.",
        Address = "London, UK",
        Location = bakuLocation
    }
},
new Company
{
    Name = "SOCAR",
    Details = new CompanyDetails
    {
        Ceo = "Rovshan Najaf",
        DateFounded = 1993,
        CompanySize = CompanySizeEnum.Size_10000Plus,
        Industry = await _context.Industries.FirstOrDefaultAsync(i => i.Name == "Energy, Mining & Utilities"),
        Link = "https://www.socar.az",
        Description = "SOCAR is the state oil company of Azerbaijan.",
        Address = "Baku, Azerbaijan",
        Location = bakuLocation
    }
},

new Company
{
    Name = "Baker McKenzie",
    Details = new CompanyDetails
    {
        Ceo = "Milton Cheng",
        DateFounded = 1949,
        CompanySize = CompanySizeEnum.Size_5001_10000,
        Industry = await _context.Industries.FirstOrDefaultAsync(i => i.Name == "Legal"),
        Link = "https://www.bakermckenzie.com",
        Description = "Baker McKenzie is an international law firm.",
        Address = "Chicago, Illinois, U.S.",
        Location = bakuLocation
    }
},
new Company
{
    Name = "McKinsey & Company",
    Details = new CompanyDetails
    {
        Ceo = "Kevin Sneader",
        DateFounded = 1926,
        CompanySize = CompanySizeEnum.Size_10000Plus,
        Industry = await _context.Industries.FirstOrDefaultAsync(i => i.Name == "Management & Consulting"),
        Link = "https://www.mckinsey.com",
        Description = "McKinsey is a management consulting firm.",
        Address = "New York City, New York, U.S.",
        Location = bakuLocation
    }
},
new Company
{
    Name = "General Electric",
    Details = new CompanyDetails
    {
        Ceo = "H. Lawrence Culp Jr.",
        DateFounded = 1892,
        CompanySize = CompanySizeEnum.Size_10000Plus,
        Industry = await _context.Industries.FirstOrDefaultAsync(i => i.Name == "Manufacturing"),
        Link = "https://www.ge.com",
        Description = "General Electric is a multinational conglomerate corporation.",
        Address = "Boston, Massachusetts, U.S.",
        Location = londonLocation
    }
},
new Company
{
    Name = "BBC",
    Details = new CompanyDetails
    {
        Ceo = "Tim Davie",
        DateFounded = 1922,
        CompanySize = CompanySizeEnum.Size_10000Plus,
        Industry = await _context.Industries.FirstOrDefaultAsync(i => i.Name == "Media & Communication"),
        Link = "https://www.bbc.co.uk",
        Description = "The BBC is a British public service broadcaster.",
        Address = "London, UK",
        Location = londonLocation
    }
},
new Company
{
    Name = "UNICEF",
    Details = new CompanyDetails
    {
        Ceo = "Henrietta H. Fore",
        DateFounded = 1946,
        CompanySize = CompanySizeEnum.Size_5001_10000,
        Industry = await _context.Industries.FirstOrDefaultAsync(i => i.Name == "Nonprofit & NGO"),
        Link = "https://www.unicef.org",
        Description = "UNICEF works in some of the world’s toughest places to reach the most disadvantaged children and adolescents.",
        Address = "New York City, New York, U.S.",
        Location = londonLocation
    }
},
            new Company
            {
                Name = "Starbucks",
                Details = new CompanyDetails
                {
                    Ceo = "Kevin Johnson",
                    DateFounded = 1971,
                    CompanySize = CompanySizeEnum.Size_10000Plus,
                    Industry = foodIndustry,
                    Link = "https://www.starbucks.com",
                    Description = "An American multinational chain of coffeehouses and roastery reserves.",
                    Address = "Seattle, Washington, U.S.",
                    Location = bakuLocation
                }
            },
                 new Company
            {
                Name = "Starbucks",
                Details = new CompanyDetails
                {
                    Ceo = "Kevin Johnson",
                    DateFounded = 1971,
                    CompanySize = CompanySizeEnum.Size_10000Plus,
                    Industry = foodIndustry,
                    Link = "https://www.starbucks.com",
                    Description = "An American multinational chain of coffeehouses and roastery reserves.",
                    Address = "Seattle, Washington, U.S.",
                    Location = londonLocation
                }
            },
            new Company
            {
                Name = "Microsoft",
                Details = new CompanyDetails
                {
                    Ceo = "Satya Nadella",
                    DateFounded = 1975,
                    CompanySize = CompanySizeEnum.Size_10000Plus,
                    Industry = techIndustry,
                    Link = "https://www.microsoft.com",
                    Description = "Microsoft develops, licenses, and supports a wide range of software products, computing devices, and services.",
                    Address = "Redmond, Washington, U.S.",
                    Location = bakuLocation
                }
            },
            new Company
            {
                Name = "Apple",
                Details = new CompanyDetails
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
            },
            new Company
            {
                Name = "Cargill",
                Details = new CompanyDetails
                {
                    Ceo = "David MacLennan",
                    DateFounded = 1865,
                    CompanySize = CompanySizeEnum.Size_10000Plus,
                    Industry = agricultureIndustry,
                    Link = "https://www.cargill.com",
                    Description = "Cargill provides food, agriculture, financial and industrial products and services.",
                    Address = "Minneapolis, Minnesota, U.S.",
                    Location = londonLocation
                }
            },
            new Company
            {
                Name = "JPMorgan Chase",
                Details = new CompanyDetails
                {
                    Ceo = "Jamie Dimon",
                    DateFounded = 2000,
                    CompanySize = CompanySizeEnum.Size_10000Plus,
                    Industry = financialIndustry,
                    Link = "https://www.jpmorganchase.com",
                    Description = "JPMorgan Chase is a multinational investment bank and financial services holding company.",
                    Address = "New York City, New York, U.S.",
                    Location = bakuLocation
                }
            },
            new Company
            {
                Name = "Pfizer",
                Details = new CompanyDetails
                {
                    Ceo = "Albert Bourla",
                    DateFounded = 1849,
                    CompanySize = CompanySizeEnum.Size_10000Plus,
                    Industry = healthcareIndustry,
                    Link = "https://www.pfizer.com",
                    Description = "Pfizer is an American multinational pharmaceutical corporation.",
                    Address = "New York City, New York, U.S.",
                    Location = londonLocation
                }
            }
        };

                await _context.Companies.AddRangeAsync(companies);
                await _context.SaveChangesAsync();
            }
        }

        public async Task SeedReviewsAsync()
        {
            List<Review> reviews = new List<Review>();
            Random rand = new Random();

            string[] companies = { "Apple", "Microsoft", "Google", "Bravo", "McDonalds", "Vodafone", "Cargill", "JPMorgan Chase", "Pfizer" };
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
