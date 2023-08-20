using AutoMapper;
using CareerCompassAPI.Application.DTOs.Vacancy_DTOs;
using CareerCompassAPI.Domain.Entities;

namespace CareerCompassAPI.Persistence.MapperProfiles
{
    public class VacancyProfile:Profile
    {
        public VacancyProfile()
        {
            CreateMap<Vacancy,VacancyGetDto>().ReverseMap();
            CreateMap<Vacancy,VacancyGetByIdDto>().ReverseMap();
        }
    }
}
