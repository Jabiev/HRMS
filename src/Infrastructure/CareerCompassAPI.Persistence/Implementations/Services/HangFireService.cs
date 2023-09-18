using CareerCompassAPI.Application.Abstraction.Repositories.IRecruiterRepositories;
using CareerCompassAPI.Application.Abstraction.Services;
using CareerCompassAPI.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace CareerCompassAPI.Persistence.Implementations.Services
{
    public class HangFireService : IHangFireService
    {
        private readonly IRecruiterReadRepository _recruiterReadRepository;
        private readonly IRecruiterWriteRepository _recruiterWriteRepository;
        private readonly CareerCompassDbContext _context;
        public HangFireService(IRecruiterReadRepository recruiterReadRepository,
                               CareerCompassDbContext context,
                               IRecruiterWriteRepository recruiterWriteRepository)
        {
            _recruiterReadRepository = recruiterReadRepository;
            _context = context;
            _recruiterWriteRepository = recruiterWriteRepository;
        }

        public async Task CheckSubscriptions()
        {

            var recruiters = await _context.Recruiters.ToListAsync();
            var freeSubscription = await _context.Subscriptions.FirstOrDefaultAsync(s => s.Price == 0);

            foreach (var recruiter in recruiters)
            {
                if (recruiter.SubscriptionStartDate.AddDays(30) < DateTime.Now)
                {
                    recruiter.CurrentPostCount = 0;
                    recruiter.Subscription = freeSubscription; 

                    _recruiterWriteRepository.Update(recruiter);
                }
            }

            await _recruiterWriteRepository.SaveChangesAsync();
        }

        public async Task DeleteOldMessages()
        {
            var daysSetting = await _context.Settings
                .Where(s => s.SettingName == "DaysToDeleteOldMessages")
                .FirstOrDefaultAsync();

            if (daysSetting != null)
            {
                int daysToDeleteOldMessages = int.Parse(daysSetting.SettingValue); 
                var olderThan = DateTime.Now.AddDays(-daysToDeleteOldMessages);
                var oldMessages = await _context.Messages.Where(m => m.DateCreated <= olderThan).ToListAsync();
                _context.Messages.RemoveRange(oldMessages);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteOldNotifications()
        {
            var daysSetting = await _context.Settings
                .Where(s => s.SettingName == "DaysToDeleteOldNotifications")
                .FirstOrDefaultAsync();

            if (daysSetting != null)
            {
                int daysToDeleteOldNotifications = int.Parse(daysSetting.SettingValue); 
                var olderThan = DateTime.Now.AddDays(-daysToDeleteOldNotifications);
                var oldNotifications = await _context.Notifications.Where(n => n.DateCreated <= olderThan).ToListAsync();
                _context.Notifications.RemoveRange(oldNotifications);
                await _context.SaveChangesAsync();
            }
        }

    }
}
