using AutoMapper;
using CareerCompassAPI.Application.DTOs.Industry_DTOs;
using CareerCompassAPI.Domain.Entities;

namespace CareerCompassAPI.Persistence.MapperProfiles
{
    public class IndustryProfile:Profile
    {
        public IndustryProfile()
        {
            CreateMap<Industry, IndustryGetDto>().ReverseMap();
        }
    }
}
