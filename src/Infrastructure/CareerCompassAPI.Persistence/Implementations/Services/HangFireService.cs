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


    }
}
