using CareerCompassAPI.Application.Abstraction.Services;
using CareerCompassAPI.Application.DTOs.AppSetting_DTOs;
using CareerCompassAPI.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace CareerCompassAPI.Persistence.Implementations.Services
{
    public class AppSettingService : IAppSettingService
    {
        private readonly CareerCompassDbContext _context;

        public AppSettingService(CareerCompassDbContext context)
        {
            _context = context;
        }
    }
}
