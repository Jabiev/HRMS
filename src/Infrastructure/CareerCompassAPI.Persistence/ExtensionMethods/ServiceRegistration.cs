using CareerCompassAPI.Application.Abstraction.Repositories.IJobSeekerRepositories;
using CareerCompassAPI.Application.Abstraction.Repositories.IRecruiterRepositories;
using CareerCompassAPI.Application.Abstraction.Repositories.ISubscriptionRepository;
using CareerCompassAPI.Application.Abstraction.Repositories;
using CareerCompassAPI.Application.Abstraction.Services;
using CareerCompassAPI.Persistence.Contexts;
using CareerCompassAPI.Persistence.Implementations.Repositories.JobSeekerRepositories;
using CareerCompassAPI.Persistence.Implementations.Repositories.RecruiterRepositories;
using CareerCompassAPI.Persistence.Implementations.Repositories.SubscriptionRepositories;
using CareerCompassAPI.Persistence.Implementations.Repositories;
using CareerCompassAPI.Persistence.Implementations.Services;
using CareerCompassAPI.Persistence.MapperProfiles;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using CareerCompassAPI.Infrastructure.Services.Token;
using CareerCompassAPI.Application.Abstraction.Repositories.ICompanyRepositories;
using CareerCompassAPI.Persistence.Implementations.Repositories.CompanyRepositories;
using CareerCompassAPI.Application.Abstraction.Repositories.IIndustryRepositories;
using CareerCompassAPI.Persistence.Implementations.Repositories.IndustryRepositories;
using CareerCompassAPI.Application.Abstraction.Repositories.INotificationRepositories;
using CareerCompassAPI.Persistence.Implementations.Repositories.NotificationRepositories;
using Hangfire;
using CareerCompassAPI.Infrastructure.Services;

namespace CareerCompassAPI.Persistence.ExtensionMethods
{
    public static class ServiceRegistration
    {
        public static void AddPersistenceServices(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddScoped<CareerCompassDbContextInitialiser>();
            AddReadRepositories(services);
            AddWriteRepositories(services);
            services.AddScoped<ISubscriptionService, SubscriptionService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<IIndustryService, IndustryService>();
            services.AddScoped<ICompanyService, CompanyService>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<IRecruiterService, RecruiterService>();
            services.AddScoped<ILocationService, LocationService>();
            services.AddScoped<IMailService, MailService>();
            services.AddAutoMapper(typeof(SubscriptionProfile).Assembly);
            services.AddDbContext<CareerCompassDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("Default")));
            services.AddHangfire(x =>
            {
                x.UseSqlServerStorage(configuration.GetConnectionString("Default"));
            });
            services.AddHangfireServer();
            services.AddHttpContextAccessor();
        }

        private static void AddReadRepositories(IServiceCollection services)
        {
            services.AddScoped(typeof(IReadRepository<>), typeof(ReadRepository<>));
            services.AddScoped<ISubscriptionReadRepository, SubscriptionReadRepository>();
            services.AddScoped<IJobSeekerReadRepository, JobSeekerReadRepository>();
            services.AddScoped<IRecruiterReadRepository, RecruiterReadRepository>();
            services.AddScoped<ICompanyReadRepository, CompanyReadRepository>();    
            services.AddScoped<IIndustryReadRepository, IndustryReadRepository>();
            services.AddScoped<INotificationReadRepository, NotificationReadRepository>();
            services.AddScoped<IRecruiterReadRepository, RecruiterReadRepository>();
        }

        private static void AddWriteRepositories(IServiceCollection services)
        {
            services.AddScoped(typeof(IWriteRepository<>), typeof(WriteRepository<>));
            services.AddScoped<ISubscriptionWriteRepository, SubscriptionWriteRepository>();
            services.AddScoped<IJobSeekerWriteRepository, JobSeekerWriteRepository>();
            services.AddScoped<IRecruiterWriteRepository, RecruiterWriteRepository>();
            services.AddScoped<ICompanyWriteRepository, CompanyWriteRepository>();
            services.AddScoped<IIndustryWriteRepository,IndustryWriteRepository>();
            services.AddScoped<INotificationWriteRepository,NotificationWriteRepository>();
            services.AddScoped<IRecruiterWriteRepository, RecruiterWriteRepository>();
        }
    }
}
