using AutoMapper;
using CareerCompassAPI.Application.DTOs.Location_DTOs;
using CareerCompassAPI.Domain.Entities;

namespace CareerCompassAPI.Persistence.MapperProfiles
{
    public class LocationProfile:Profile
    {
        public LocationProfile()
        {
            CreateMap<JobLocation, LocationGetDto>().ReverseMap();
        }
    }
}
