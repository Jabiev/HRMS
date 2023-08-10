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
            services.AddAutoMapper(typeof(SubscriptionProfile).Assembly);
            services.AddDbContext<CareerCompassDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("Default")));
        }

        private static void AddReadRepositories(IServiceCollection services)
        {
            services.AddScoped(typeof(IReadRepository<>), typeof(ReadRepository<>));
            services.AddScoped<ISubscriptionReadRepository, SubscriptionReadRepository>();
            services.AddScoped<IJobSeekerReadRepository, JobSeekerReadRepository>();
            services.AddScoped<IRecruiterReadRepository, RecruiterReadRepository>();
        }

        private static void AddWriteRepositories(IServiceCollection services)
        {
            services.AddScoped(typeof(IWriteRepository<>), typeof(WriteRepository<>));
            services.AddScoped<ISubscriptionWriteRepository, SubscriptionWriteRepository>();
            services.AddScoped<IJobSeekerWriteRepository, JobSeekerWriteRepository>();
            services.AddScoped<IRecruiterWriteRepository, RecruiterWriteRepository>();
        }
    }
}
