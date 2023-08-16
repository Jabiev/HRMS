using AutoMapper;
using CareerCompassAPI.Application.DTOs.Company_DTOs;
using CareerCompassAPI.Domain.Entities;

namespace CareerCompassAPI.Persistence.MapperProfiles
{
    public class CompanyProfile:Profile
    {
        public CompanyProfile()
        {
            CreateMap<Company, CompanyGetDto>().ReverseMap();
        }
    }
}
