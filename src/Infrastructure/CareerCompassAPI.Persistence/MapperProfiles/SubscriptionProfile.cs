using AutoMapper;
using CareerCompassAPI.Application.DTOs.Subscription_DTOs;
using CareerCompassAPI.Domain.Entities;

namespace CareerCompassAPI.Persistence.MapperProfiles
{
    public class SubscriptionProfile:Profile
    {
        public SubscriptionProfile()
        {
            CreateMap<Subscriptions,SubscriptionCreateDto>().ReverseMap();
        }
    }
}
