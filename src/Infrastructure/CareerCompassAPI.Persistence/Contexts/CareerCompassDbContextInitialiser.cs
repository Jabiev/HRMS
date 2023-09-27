using CareerCompassAPI.Application.Abstraction.Repositories.ISubscriptionRepository;
using CareerCompassAPI.Domain.Entities;
using CareerCompassAPI.Domain.Enums;
using CareerCompassAPI.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

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
        private readonly ILogger<CareerCompassDbContextInitialiser> _logger;

        public CareerCompassDbContextInitialiser(CareerCompassDbContext context,
                                                 UserManager<AppUser> userManager,
                                                 RoleManager<IdentityRole> roleManager,
                                                 IConfiguration configuration,
                                                 ISubscriptionReadRepository subscriptionReadRepository,
                                                 ISubscriptionWriteRepository subscriptionWriteRepository,
                                                 ILogger<CareerCompassDbContextInitialiser> logger)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _subscriptionReadRepository = subscriptionReadRepository;
            _subscriptionWriteRepository = subscriptionWriteRepository;
            _logger = logger;
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
            if (await _context.JobSeekers.AnyAsync())
            {
                return; 
            }

            Random rand = new Random();
            List<JobSeeker> jobSeekers = new List<JobSeeker>();
            for (int i = 0; i < 550; i++)
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
            if (await _context.Recruiters.AnyAsync())
            {
                return; 
            }
            Random rand = new Random();
            List<Recruiter> recruiters = new List<Recruiter>();

            var freeSubscription = await _context.Subscriptions.FirstOrDefaultAsync(s => s.Name == "Free");
            var basicSubscription = await _context.Subscriptions.FirstOrDefaultAsync(s => s.Name == "Basic");
            var proSubscription = await _context.Subscriptions.FirstOrDefaultAsync(s => s.Name == "Pro");
            Subscriptions[] possibleSubscriptions = { freeSubscription, basicSubscription, proSubscription };

            var companies = await _context.Companies.ToListAsync();

            for (int i = 0; i < 200; i++)
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
            if (await _context.Vacancy.AnyAsync())
            {
                return; 
            }
            var allShiftsAndSchedules = await _context.ShiftAndSchedules.ToListAsync();
            Random rand = new Random();

            var recruiters = await _context.Recruiters.ToListAsync();
            var entryLevel = await _context.ExperienceLevels.FirstOrDefaultAsync(e => e.LevelName == "Entry");
            var midLevel = await _context.ExperienceLevels.FirstOrDefaultAsync(e => e.LevelName == "Mid");
            var seniorLevel = await _context.ExperienceLevels.FirstOrDefaultAsync(e => e.LevelName == "Senior");
            var noExperience = await _context.ExperienceLevels.FirstOrDefaultAsync(e => e.LevelName == "No Experience");
            var partTime = await _context.JobTypes.FirstOrDefaultAsync(j => j.TypeName == "PartTime");
            var fullTime = await _context.JobTypes.FirstOrDefaultAsync(j => j.TypeName == "FullTime");
            var contract = await _context.JobTypes.FirstOrDefaultAsync(j => j.TypeName == "Contract");
            var internship = await _context.JobTypes.FirstOrDefaultAsync(j => j.TypeName == "Internship");
            var temporary = await _context.JobTypes.FirstOrDefaultAsync(j => j.TypeName == "Temporary");
            var london = await _context.JobLocations.FirstOrDefaultAsync(l => l.Location == "London,UK");
            var baku = await _context.JobLocations.FirstOrDefaultAsync(l => l.Location == "Baku,Azerbaijan");
            var istanbul = await _context.JobLocations.FirstOrDefaultAsync(l => l.Location == "Istanbul,Turkey");
            var google = await _context.Companies.FirstOrDefaultAsync(c => c.Name == "Google");
            var microsoft = await _context.Companies.FirstOrDefaultAsync(c => c.Name == "Microsoft");
            var starbucks = await _context.Companies.FirstOrDefaultAsync(c => c.Name == "Starbucks");
            var rockstar = await _context.Companies.FirstOrDefaultAsync(c => c.Name == "Rockstar Games");
            var fedex = await _context.Companies.FirstOrDefaultAsync(c => c.Name == "FedEx");
            var marriott = await _context.Companies.FirstOrDefaultAsync(c => c.Name == "Marriott International");
            var socar = await _context.Companies.FirstOrDefaultAsync(c => c.Name == "SOCAR");
            var jpmorgan = await _context.Companies.FirstOrDefaultAsync(c => c.Name == "JPMorgan Chase");
            List<Vacancy> vacancies = new List<Vacancy>
            {
                new Vacancy
                {
                    JobTitle = "Finance Manager",
                    ExperienceLevel = midLevel,
                    JobType = new List<JobType>
                    {
                        partTime,
                        contract,
                    },
                    Recruiter = recruiters[rand.Next(recruiters.Count)],
                    Salary = 80000.00M,
                    JobLocation = london,
                    Description = "Responsible for managing the financial aspects of our London branch.",
                    Company = jpmorgan,
                    ApplicationLimit = 50,
                    CurrentApplicationCount = 0,
                    ShiftAndSchedules = new List<ShiftAndSchedule>
                    {
                        allShiftsAndSchedules[0],
                    }
                },
                new Vacancy
                {
                    JobTitle = "Software Engineer",
                    ExperienceLevel = entryLevel,
                      JobType = new List<JobType>
                    {
                        partTime,
                        contract,
                    },
                    Recruiter = recruiters[rand.Next(recruiters.Count)],
                    Salary = 100000.00M,
                    JobLocation = baku,
                    Description = "Software Engineer position in the Baku office focusing on web technologies.",
                    Company = google,
                    ApplicationLimit = 100,
                    CurrentApplicationCount = 0,
                     ShiftAndSchedules = new List<ShiftAndSchedule>
                    {
                        allShiftsAndSchedules[1],
                        allShiftsAndSchedules[2],
                        allShiftsAndSchedules[3],
                    }
                },
                new Vacancy
                {
                    JobTitle = "Intern Engineer",
                    ExperienceLevel = entryLevel,
                    Recruiter =  recruiters[rand.Next(recruiters.Count)],
                    Salary = 300,
                      JobType = new List<JobType>
                    {
                        internship,
                    },
                    JobLocation = baku,
                    Description = " \"We are looking for enthusiastic and motivated Intern Engineers to join our team at SOCAR. As an Intern Engineer, you will have the opportunity to gain hands-on experience in various aspects of engineering within our dynamic organization. This internship program is designed to provide you with valuable skills and knowledge that will kickstart your career in the engineering field.\"\r\n\r\nJob Responsibilities:\r\n- Collaborate with experienced engineers on projects and tasks.\r\n- Assist in research, design, and development activities.\r\n- Participate in problem-solving and troubleshooting.\r\n- Learn and apply engineering principles in real-world scenarios.\r\n- Contribute to team meetings and discussions.\r\n- Take initiative in learning and adapting to new technologies and tools.\r\n\r\nQualifications:\r\n- Pursuing a degree in Engineering or a related field.\r\n- Strong passion for engineering and a desire to learn.\r\n- Excellent problem-solving and analytical skills.\r\n- Good communication and teamwork abilities.\r\n- Eagerness to take on challenges and grow professionally.\r\n\r\nBenefits:\r\n- Hands-on experience in a leading engineering organization.\r\n- Mentorship and guidance from experienced engineers.\r\n- Exposure to cutting-edge technologies and projects.\r\n- Competitive compensation and potential for future opportunities.\r\n- Networking and career development opportunities.",
                    Company = socar,
                    ApplicationLimit = 50,
                    CurrentApplicationCount= 0,
                     ShiftAndSchedules = new List<ShiftAndSchedule>
                    {
                        allShiftsAndSchedules[0],
                        allShiftsAndSchedules[2],
                    }
                },
                new Vacancy
                {
                    JobTitle = "Junior Game Developer",
                    ExperienceLevel = entryLevel,
                      JobType = new List<JobType>
                    {
                        temporary,
                        contract,
                    },
                    Recruiter = recruiters[rand.Next(recruiters.Count)],
                    Salary = 1000.00M,
                    JobLocation = baku,
                    Description = " \"Are you a passionate gamer with a knack for game development? Rockstar Games is seeking a Junior Game Developer to join our team at our Baku office. As a Junior Game Developer, you'll have the exciting opportunity to contribute to the creation of immersive and innovative gaming experiences that captivate players worldwide.\"\r\n\r\nJob Responsibilities:\r\n- Collaborate with the game development team to design and implement game features.\r\n- Assist in coding, debugging, and optimizing game systems.\r\n- Work on gameplay mechanics, graphics, and user interface elements.\r\n- Participate in brainstorming sessions and contribute creative ideas.\r\n- Help create and maintain game documentation.\r\n- Adapt to and learn new technologies and tools in the gaming industry.\r\n\r\nQualifications:\r\n- A passion for video games and a desire to work in the gaming industry.\r\n- Strong programming skills in languages like C++, C#, or similar.\r\n- Basic knowledge of game development concepts and tools.\r\n- Creativity and problem-solving skills.\r\n- Effective communication and teamwork abilities.\r\n- Willingness to learn and adapt to a fast-paced environment.\r\n\r\nBenefits:\r\n- Hands-on experience in game development with a renowned industry leader.\r\n- Mentorship and guidance from experienced game developers.\r\n- Exposure to cutting-edge game development technologies.\r\n- Competitive salary and potential for career growth.\r\n- A collaborative and fun work environment for gaming enthusiasts.\r\n\r\nIf you're an aspiring game developer looking to kickstart your career in the gaming industry and want to be part of exciting projects at Rockstar Games, we encourage you to apply for this Junior Game Developer position today. Join us in creating the next gaming masterpiece!\"\r\n\r\nFeel free to tailor this description to fit Rockstar's specific culture and requirements. If you have any further questions or need more assistance, Rahil, don't hesitate to ask. We're here to optimize and improve your code and project together, like two friends working on it!\r\n",
                    Company = rockstar,
                    ApplicationLimit = 10,
                    CurrentApplicationCount = 0,
                     ShiftAndSchedules = new List<ShiftAndSchedule>
                    {
                        allShiftsAndSchedules[0],
                    }
                },
                new Vacancy
                {
                    JobTitle = "Barista",
                    ExperienceLevel = seniorLevel,
                    Recruiter = recruiters[rand.Next(recruiters.Count)],
                    Salary = 1500,
                      JobType = new List<JobType>
                    {
                        fullTime,
                        contract,
                    },
                    JobLocation = baku,
                    Description= " \"Join the Starbucks team in Baku as a Barista and become a part of a global coffeehouse chain that's known for its commitment to quality and customer service. As a Barista at Starbucks, you'll play a crucial role in creating exceptional coffee and beverage experiences for our customers.\"\r\n\r\nJob Responsibilities:\r\n- Prepare and serve a variety of coffee and beverage orders with precision and care.\r\n- Maintain a clean and organized work environment.\r\n- Provide friendly and efficient customer service.\r\n- Educate customers about our coffee offerings and menu options.\r\n- Handle cash register transactions accurately.\r\n- Ensure that Starbucks' high-quality standards are consistently met.\r\n\r\nQualifications:\r\n- Previous experience as a Barista or in a similar role is a plus but not required.\r\n- Passion for coffee and a desire to learn about coffee preparation.\r\n- Excellent customer service and communication skills.\r\n- Ability to work in a fast-paced environment.\r\n- Strong attention to detail and cleanliness.\r\n- Positive attitude and a team player mentality.\r\n\r\nBenefits:\r\n- Comprehensive training in coffee preparation and customer service.\r\n- Competitive salary and tips.\r\n- Opportunities for career advancement within Starbucks.\r\n- A friendly and inclusive work environment.\r\n- Discounts on Starbucks products.\r\n- Flexible scheduling options.\r\n\r\nIf you have a passion for coffee and enjoy creating memorable experiences for customers, we invite you to apply for the Barista position at Starbucks. Join us in the art of crafting exceptional coffee moments!\"\r\n\r\nFeel free to customize this description to align with Starbucks' specific requirements and company culture. If you have any more questions or need further assistance, Rahil, just let me know. We're here to help you optimize your project, and you can count on me as your coding buddy!\r\n",
                    Company = starbucks,
                    ApplicationLimit = 20,
                    CurrentApplicationCount = 0,
                     ShiftAndSchedules = new List<ShiftAndSchedule>
                    {
                        allShiftsAndSchedules[0],
                        allShiftsAndSchedules[7],
                        allShiftsAndSchedules[8],
                    }

                },
                new Vacancy
                {
                    JobTitle = "Store Manager",
                    ExperienceLevel = seniorLevel,
                    Recruiter = recruiters[rand.Next(recruiters.Count)],
                    Salary = 5500,
                    JobLocation = baku,
                    Description= " \"Join Starbucks as a Store Manager and lead a team of passionate coffee enthusiasts at our Baku location. As a Store Manager, you will be responsible for the overall management and success of the store, ensuring that every customer enjoys an exceptional Starbucks experience.\"\r\n\r\nJob Responsibilities:\r\n- Lead and inspire a team of baristas and shift supervisors.\r\n- Oversee daily store operations, including staffing, scheduling, and inventory management.\r\n- Create a welcoming and customer-focused atmosphere in the store.\r\n- Maintain Starbucks' high standards of product quality and customer service.\r\n- Drive sales and meet store performance targets.\r\n- Foster a positive and inclusive work environment.\r\n- Ensure compliance with company policies and procedures.\r\n\r\nQualifications:\r\n- Previous experience in retail or restaurant management is preferred.\r\n- Strong leadership and team management skills.\r\n- Excellent customer service and communication abilities.\r\n- Problem-solving and decision-making skills.\r\n- Financial acumen for managing store budgets and expenses.\r\n- A passion for coffee and Starbucks' mission.\r\n- Willingness to work flexible hours, including weekends and evenings.\r\n\r\nBenefits:\r\n- Competitive salary and performance-based bonuses.\r\n- Comprehensive training and career development opportunities.\r\n- Medical, dental, and vision benefits.\r\n- 401(k) retirement savings plan.\r\n- Paid time off and vacation benefits.\r\n- A supportive and collaborative work environment.\r\n- Discounts on Starbucks products.\r\n\r\nIf you're a dynamic and experienced leader looking to take your career to the next level with a globally recognized brand like Starbucks, we encourage you to apply for the Store Manager position. Join us in delivering the Starbucks experience and making every visit memorable for our customers!\"\r\n\r\nFeel free to adjust this description to align with Starbucks' specific expectations and company culture. If you have any more questions or need further assistance, Rahil, don't hesitate to ask. We're here to help you optimize your project and code, just like two friends working together!\r\n",
                    Company = starbucks,
                    ApplicationLimit = 20,
                    CurrentApplicationCount = 0,
                     ShiftAndSchedules = new List<ShiftAndSchedule>
                    {
                        allShiftsAndSchedules[0],
                        allShiftsAndSchedules[6],
                        allShiftsAndSchedules[4],
                    }
                },
                new Vacancy
                {
                 JobTitle = "Software Engineer",
                ExperienceLevel = midLevel,
                  JobType = new List<JobType>
                    {
                        fullTime,
                        contract,
                    },
                Recruiter = recruiters[rand.Next(recruiters.Count)],
                Salary = 70000.00M,
                JobLocation = london,
                Description = "Join Microsoft's London office as a Software Engineer and be part of          a dynamic team working on cutting-edge software projects. As a Software Engineer           , you'll have the opportunity to design, develop, and maintain software solutions that impact millions of users worldwide.",
                 Company = microsoft,
                 ApplicationLimit = 30,
                 CurrentApplicationCount = 0,
                  ShiftAndSchedules = new List<ShiftAndSchedule>
                    {
                        allShiftsAndSchedules[0],
                        allShiftsAndSchedules[2],
                    }
                },
                new Vacancy
{
    JobTitle = "Data Scientist",
    ExperienceLevel = midLevel,
    Recruiter = recruiters[rand.Next(recruiters.Count)],
          JobType = new List<JobType>
                    {
                        fullTime,
                        contract,
                    },
    Salary = 75000.00M,
    JobLocation = london,
    Description = "Microsoft's London office is seeking a talented Data Scientist to join our team. As a Data Scientist, you will leverage data analytics and machine learning techniques to extract valuable insights and drive data-driven decision-making within the company.",
    Company = microsoft,
    ApplicationLimit = 25,
    CurrentApplicationCount = 0,
     ShiftAndSchedules = new List<ShiftAndSchedule>
                    {
                        allShiftsAndSchedules[4],
                        allShiftsAndSchedules[7],
                    }
},
                new Vacancy
{
    JobTitle = "Cloud Solutions Architect",
    ExperienceLevel = seniorLevel,
    Recruiter = recruiters[rand.Next(recruiters.Count)],
    Salary = 90000.00M,
          JobType = new List<JobType>
                    {
                        fullTime,
                        contract,
                    },
    JobLocation = baku,
    Description = "Microsoft's Baku office is looking for a Cloud Solutions Architect to lead the design and implementation of cloud-based solutions for our clients. As a Cloud Solutions Architect, you'll collaborate with customers to define cloud strategies and deliver innovative solutions.",
    Company = microsoft,
    ApplicationLimit = 20,
    CurrentApplicationCount = 0,
     ShiftAndSchedules = new List<ShiftAndSchedule>
                    {
                        allShiftsAndSchedules[0],
                        allShiftsAndSchedules[8],
                    }
},
                new Vacancy
{
    JobTitle = "Product Manager",
    ExperienceLevel = midLevel,
    Recruiter = recruiters[rand.Next(recruiters.Count)],
    Salary = 80000.00M,
          JobType = new List<JobType>
                    {
                        fullTime,
                        contract,
                    },
    JobLocation = baku,
    Description = "Join Microsoft's Baku office as a Product Manager and take ownership of the product development lifecycle. As a Product Manager, you will define product roadmaps, gather user feedback, and collaborate with cross-functional teams to deliver exceptional software products.",
    Company = microsoft,
    ApplicationLimit = 15,
    CurrentApplicationCount = 0,
     ShiftAndSchedules = new List<ShiftAndSchedule>
                    {
                        allShiftsAndSchedules[0],
                        allShiftsAndSchedules[2],
                    }
},
                new Vacancy
                {
                     JobTitle = "Senior Frontend Developer",
    ExperienceLevel = seniorLevel,
    Recruiter = recruiters[rand.Next(recruiters.Count)], 
    Salary = 60000.00M,
    JobLocation = baku,
    Description = "Marriott International in Baku is seeking a Senior Frontend Developer to lead the development of user interfaces for our web applications.",
    Company = marriott,
    ApplicationLimit = 20,
    CurrentApplicationCount = 0,
     ShiftAndSchedules = new List<ShiftAndSchedule>
                    {
                        allShiftsAndSchedules[1],
                        allShiftsAndSchedules[7],
                    },
    JobType = new List<JobType> { fullTime }
                },
                new Vacancy
                {
                     JobTitle = "Contract Project Manager",
    ExperienceLevel = midLevel,
    Recruiter = recruiters[rand.Next(recruiters.Count)],
    Salary = 75000.00M, 
    JobLocation = baku,
    Description = "Marriott International in Baku is seeking a Contract Project Manager to oversee and deliver key projects on a contractual basis.",
    Company = marriott,
    ApplicationLimit = 15, 
    CurrentApplicationCount = 0,
     ShiftAndSchedules = new List<ShiftAndSchedule>
                    {
                        allShiftsAndSchedules[1],
                        allShiftsAndSchedules[2],
                    },
    JobType = new List<JobType> { contract }
                },
                new Vacancy
                {
                    JobTitle = "Delivery Driver",
    ExperienceLevel = entryLevel, 
    Recruiter = recruiters[rand.Next(recruiters.Count)],
    Salary = 30000.00M,
    JobLocation = baku,
    Description = "FedEx in Baku is hiring Delivery Drivers to join our team. As a Delivery Driver, you will be responsible for delivering packages to our customers with a focus on safety and efficiency.",
    Company = fedex,
    ApplicationLimit = 15,
    CurrentApplicationCount = 0,
    JobType = new List<JobType> { fullTime }
                },
                new Vacancy
                {
                    JobTitle = "Logistics Manager",
    ExperienceLevel = seniorLevel, 
    Recruiter =recruiters[rand.Next(recruiters.Count)],
    Salary = 80000.00M, // Set the salary.
    JobLocation = london,
    Description = "FedEx in London is seeking a Logistics Manager to oversee and optimize our logistics operations. This role involves managing teams and ensuring the efficient flow of goods.",
    Company = fedex,
    ApplicationLimit = 10, 
    CurrentApplicationCount = 0,
    JobType = new List<JobType> { fullTime },
     ShiftAndSchedules = new List<ShiftAndSchedule>
                    {
                        allShiftsAndSchedules[0],
                        allShiftsAndSchedules[2],
                    }
                }
            };


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
            if (await _context.Reviews.AnyAsync())
            {
                return;
            }
            List<Review> reviews = new List<Review>();
            Random rand = new Random();

            string[] companies = { "Apple", "Microsoft", "Google", "Bravo", "McDonalds", "Vodafone", "Cargill", "JPMorgan Chase", "Pfizer", "FedEx", "Marriott International", "State Farm", "Rockstar Games", "Ubisoft", "CD Projekt", "Icherisheher Art Gallery", "Balfour Beatty", "BP", "SOCAR", "Baker McKenzie", "McKinsey & Company", "General Electric", "BBC", "UNICEF", "Starbucks" };
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
                new AppSetting {SettingName = "DaysToDeleteFullVacancies", SettingValue = "3"},
                new AppSetting {SettingName = "DaysToDeleteOldVacancies", SettingValue ="30"},
                new AppSetting {SettingName = "ContactMainEmail", SettingValue ="infoccompass@gmail.com"},
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

        public async Task PaymentsSeedAsync()
        {
            try
            {
                _logger.LogInformation("Starting PaymentsSeedAsync");

                var random = new Random();
                var paymentTypes = Enum.GetValues(typeof(PaymentTypes)).Cast<PaymentTypes>().ToList();

                _logger.LogInformation("Fetching app users...");
                var appUsers = await _context.Users.ToListAsync();

                if (appUsers == null || appUsers.Count == 0)
                {
                    _logger.LogWarning("No app users found. Exiting PaymentsSeedAsync.");
                    return;
                }

                _logger.LogInformation($"Fetched {appUsers.Count} app users");

                var payments = new List<Payments>();

                foreach (var appUser in appUsers)
                {
                    _logger.LogInformation($"Creating payments for user: {appUser.Id}");

                    for (int i = 0; i < random.Next(1, 6); i++)
                    {
                        var paymentType = paymentTypes[random.Next(paymentTypes.Count)];
                        _logger.LogInformation($"Selected payment type: {paymentType}");

                        var amount = paymentType switch
                        {
                            PaymentTypes.Resume => 14.99M,
                            PaymentTypes.Subscription => random.Next(0, 2) == 0 ? 149M : 349M,
                            _ => throw new InvalidOperationException("Unknown payment type"),
                        };

                        _logger.LogInformation($"Generated payment amount: {amount}");

                        payments.Add(new Payments
                        {
                            AppUser = appUser,
                            Type = paymentType,
                            Amount = amount
                        });

                        _logger.LogInformation($"Added payment for user {appUser.Id} with type {paymentType} and amount {amount}");
                    }
                }

                _logger.LogInformation("Adding generated payments to context...");
                await _context.Payments.AddRangeAsync(payments);

                _logger.LogInformation("Saving changes...");
                await _context.SaveChangesAsync();

                _logger.LogInformation("Successfully completed PaymentsSeedAsync");
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while seeding Payments: {ex}");
            }
        }




    }
}
