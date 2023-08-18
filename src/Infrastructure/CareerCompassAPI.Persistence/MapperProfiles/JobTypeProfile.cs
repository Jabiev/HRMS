using AutoMapper;
using CareerCompassAPI.Application.DTOs.JobType_DTOs;
using CareerCompassAPI.Domain.Entities;

namespace CareerCompassAPI.Persistence.MapperProfiles
{
    public class JobTypeProfile:Profile
    {
        public JobTypeProfile()
        {
            CreateMap<JobType, JobTypeGetDto>().ReverseMap();
        }
    }
}
