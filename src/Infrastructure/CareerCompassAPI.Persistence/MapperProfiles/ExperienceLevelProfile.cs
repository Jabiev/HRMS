using AutoMapper;
using CareerCompassAPI.Application.DTOs.ExperienceLevel_DTOs;
using CareerCompassAPI.Domain.Entities;

namespace CareerCompassAPI.Persistence.MapperProfiles
{
    public class ExperienceLevelProfile:Profile
    {
        public ExperienceLevelProfile()
        {
            CreateMap<ExperienceLevel, ExperienceLevelGetDto>().ReverseMap();
        }
    }
}
