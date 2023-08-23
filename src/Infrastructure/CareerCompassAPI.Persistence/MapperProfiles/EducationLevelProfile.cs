using AutoMapper;
using CareerCompassAPI.Application.DTOs.EducationLevel_DTOs;
using CareerCompassAPI.Domain.Entities;

namespace CareerCompassAPI.Persistence.MapperProfiles
{
    public class EducationLevelProfile:Profile
    {
        public EducationLevelProfile()
        {
            CreateMap<EducationLevel, EducationLevelGetDto>().ReverseMap();
        }
    }
}
