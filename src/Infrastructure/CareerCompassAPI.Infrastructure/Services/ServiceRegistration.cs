using CareerCompassAPI.Application.Abstraction.Services;
using CareerCompassAPI.Application.Abstraction.Storage;
using CareerCompassAPI.Infrastructure.Services.Token;
using Microsoft.Extensions.DependencyInjection;

namespace CareerCompassAPI.Infrastructure.Services
{
    public static class ServiceRegistration
    {
        public static void AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddScoped<IJwtService,JwtService>();
            services.AddScoped<IStorageService,StorageService>();
        }
        public static void AddStorage<T>(this  IServiceCollection services) where T : class , IStorage
        {
            services.AddScoped<IStorage, T>();
        }
    }
}
