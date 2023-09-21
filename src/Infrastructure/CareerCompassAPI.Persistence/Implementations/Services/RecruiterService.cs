using CareerCompassAPI.Application.Abstraction.Repositories.IRecruiterRepositories;
using CareerCompassAPI.Application.Abstraction.Services;
using CareerCompassAPI.Application.DTOs.FeatureAccess_DTO;
using CareerCompassAPI.Application.DTOs.Recruiter_DTOs;
using CareerCompassAPI.Domain.Entities;
using CareerCompassAPI.Persistence.Contexts;
using CareerCompassAPI.Persistence.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace CareerCompassAPI.Persistence.Implementations.Services
{
    public class RecruiterService : IRecruiterService
    {
        private readonly IRecruiterReadRepository _recruiterReadRepository;
        private readonly CareerCompassDbContext _context;
        public RecruiterService(IRecruiterReadRepository recruiterReadRepository,
                                CareerCompassDbContext context)
        {
            _recruiterReadRepository = recruiterReadRepository;
            _context = context;
        }
        public async Task<RecruiterGetDto> GetRecruiterByUserId(Guid userId)
        {
            Recruiter recruiter = await _recruiterReadRepository.GetByUserIdAsync(userId);
            if (recruiter is not Recruiter)
            {
                throw new ArgumentNullException();
            }
            Guid subscriptionId = recruiter.Subscription.Id;

            RecruiterGetDto recruiterGetDto = new(
                id: recruiter.Id,
                AppUserId: recruiter.AppUserId,
                CompanyId: recruiter.CompanyId,
                FirstName: recruiter.FirstName,
                LastName: recruiter.LastName,
                SubscriptionId: subscriptionId,
                SubscriptionStartDate: recruiter.SubscriptionStartDate
            );
            return recruiterGetDto;
        }

        public async Task<Subscriptions> GetSubscriptionForRecruiter(Guid appUserId)
        {
            var recruiter = await _recruiterReadRepository.GetByUserIdAsync(appUserId);
            if (recruiter is null)
            {
                throw new NotFoundException("User not found");
            }
            var subscription = await _context.Subscriptions.FirstOrDefaultAsync(s => s.Id == recruiter.Subscription.Id);
            return subscription;
        }
        public async Task<FeatureAccessGetDto> GetAvailableFeaturesForRecruiter(Guid appUserId)
        {
            var subscription = await GetSubscriptionForRecruiter(appUserId);
            return new FeatureAccessGetDto(subscription.isPlannerAvailable, subscription.isVideoAvailable, subscription.isChatAvailable);
        }

    }
}
