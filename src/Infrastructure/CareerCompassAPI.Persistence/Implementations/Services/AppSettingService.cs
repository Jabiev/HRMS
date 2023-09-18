using CareerCompassAPI.Application.Abstraction.Services;
using CareerCompassAPI.Application.DTOs.AppSetting_DTOs;
using CareerCompassAPI.Persistence.Contexts;
using CareerCompassAPI.Persistence.Exceptions;
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

        public async Task UpdateSettingValue(AppSettingMessageUpdateDto update)
        {
            var setting = await _context.Settings
                .FirstOrDefaultAsync(s => s.Id == update.settingId);
            if (setting == null)
            {
                throw new NotFoundException($"Setting with given Id does not exist");
            }
            setting.SettingValue = update.settingValue;
            await _context.SaveChangesAsync();
        }
    }
}
