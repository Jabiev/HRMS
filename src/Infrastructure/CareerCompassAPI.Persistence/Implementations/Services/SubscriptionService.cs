using AutoMapper;
using CareerCompassAPI.Application.Abstraction.Repositories.ISubscriptionRepository;
using CareerCompassAPI.Application.Abstraction.Services;
using CareerCompassAPI.Application.DTOs.Subscription_DTOs;
using CareerCompassAPI.Domain.Entities;

namespace CareerCompassAPI.Persistence.Implementations.Services
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly ISubscriptionReadRepository _subscriptionReadRepository;
        private readonly ISubscriptionWriteRepository _subscriptionWriteRepository;
        private readonly IMapper _mapper;

        public SubscriptionService(ISubscriptionReadRepository subscriptionReadRepository,
                                   ISubscriptionWriteRepository subscriptionWriteRepository,
                                   IMapper mapper)
        {
            _subscriptionReadRepository = subscriptionReadRepository;
            _subscriptionWriteRepository = subscriptionWriteRepository;
            _mapper = mapper;
        }

        public async Task CreateAsync(SubscriptionCreateDto subscriptionCreateDto)
        {
            if (await _subscriptionReadRepository.GetByExpressionAsync(s => s.Name.ToLower() == subscriptionCreateDto.Name.ToLower())is not null)
            {
                throw new Exception();
            }
            var newSubscription = _mapper.Map<Subscriptions>(subscriptionCreateDto);
            await _subscriptionWriteRepository.AddAsync(newSubscription);
            await _subscriptionWriteRepository.SaveChangesAsync();
        }

        public async Task<List<SubscriptionGetDto>> GetAllAsync()
        {
            var subscriptionList = await _subscriptionReadRepository.GetAllAsync();
            List<SubscriptionGetDto> list = _mapper.Map<List<SubscriptionGetDto>>(subscriptionList);
            return list;
        }
    }
}
