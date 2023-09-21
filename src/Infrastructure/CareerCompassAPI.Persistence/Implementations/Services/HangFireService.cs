using CareerCompassAPI.Application.Abstraction.Repositories.IRecruiterRepositories;
using CareerCompassAPI.Application.Abstraction.Services;
using CareerCompassAPI.Domain.Enums;
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

        public async Task DeleteDeclinedApplications()
        {
            var daysSetting = await _context.Settings
                .Where(s => s.SettingName == "DaysToDeleteDeclinedApplications")
                .FirstOrDefaultAsync();
            if (daysSetting != null && int.TryParse(daysSetting.SettingValue, out int days))
            {
                DateTime cutoffDate = DateTime.Now.AddDays(-days);
                var declinedApplications = await _context.Applications
                    .Where(ja => ja.Status == ApplicationStatus.Declined && ja.DateCreated < cutoffDate)
                    .ToListAsync();
                _context.Applications.RemoveRange(declinedApplications);
                await _context.SaveChangesAsync();
            }

        }

        public async Task DeleteDeclinedReviews()
        {
            var daysSetting = await _context.Settings
                .Where(s => s.SettingName == "DaysToDeleteDeclinedReviews")
                .FirstOrDefaultAsync();

            int daysToKeep = int.Parse(daysSetting?.SettingValue ?? "3"); 
            DateTime cutoffDate = DateTime.UtcNow.AddDays(-daysToKeep);
            var reviewsToDelete = await _context.Reviews
                .Where(r => r.Status == ReviewStatus.Declined && r.DateCreated < cutoffDate)
                .ToListAsync();
            _context.Reviews.RemoveRange(reviewsToDelete);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteFullVacancies()
        {
            var daysSetting = await _context.Settings
                .Where(s => s.SettingName == "DaysToDeleteFullVacancies")
                .FirstOrDefaultAsync();
            int daysToDeleteFullVacancies = daysSetting != null ? int.Parse(daysSetting.SettingValue) : 3;
            var olderThan = DateTime.UtcNow.AddDays(-daysToDeleteFullVacancies);
            var fullVacancies = await _context.Vacancy
                .Where(v => v.ApplicationLimit == v.CurrentApplicationCount && v.DateCreated <= olderThan)
                .ToListAsync(); 
            _context.Vacancy.RemoveRange(fullVacancies);
            await _context.SaveChangesAsync();  
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

        public async Task DeleteOldVacancies()
        {
            var daysSetting = await _context.Settings
                .Where(s => s.SettingName == "DaysToDeleteOldVacancies")
                .FirstOrDefaultAsync();
            int daysToDeleteOldVacancies = daysSetting != null ? int.Parse(daysSetting.SettingValue) : 30;
            var cutoffDate = DateTime.Now.AddDays(-daysToDeleteOldVacancies);
            var oldVacancies = await _context.Vacancy
                .Where(v => v.DateCreated <= cutoffDate && !v.IsDeleted)
                .ToListAsync();
            foreach (var vacancy in oldVacancies)
            {
                vacancy.IsDeleted = true;
            }
            await _context.SaveChangesAsync();
        }

    }
}
