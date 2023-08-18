using AutoMapper;
using CareerCompassAPI.Application.DTOs.Schedule_DTOs;
using CareerCompassAPI.Domain.Entities;

namespace CareerCompassAPI.Persistence.MapperProfiles
{
    public class ScheduleProfile:Profile
    {
        public ScheduleProfile()
        {
            CreateMap<ShiftAndSchedule,ShiftAndScheduleGetDto>().ReverseMap();
        }
    }
}
