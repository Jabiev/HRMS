using CareerCompassAPI.Application.DTOs.EducationLevel_DTOs;
using CareerCompassAPI.Application.DTOs.ExperienceLevel_DTOs;

namespace CareerCompassAPI.Application.Abstraction.Services
{
    public interface IEducationLevelService
    {
        Task<List<EducationLevelGetDto>> GetAllAsync();
    }
}
