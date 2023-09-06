using AutoMapper;
using CareerCompassAPI.Application.DTOs.EducationLevel_DTOs;
using CareerCompassAPI.Application.DTOs.Payment_DTOs;
using CareerCompassAPI.Domain.Entities;

namespace CareerCompassAPI.Persistence.MapperProfiles
{
    public class PaymentsProfile:Profile
    {
        public PaymentsProfile()
        {
            CreateMap<Payments, PaymentsGetDto>().ReverseMap();
        }
    }
}
