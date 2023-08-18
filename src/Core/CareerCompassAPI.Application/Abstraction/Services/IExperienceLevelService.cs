using CareerCompassAPI.Application.DTOs.ExperienceLevel_DTOs;
using CareerCompassAPI.Application.DTOs.Industry_DTOs;

namespace CareerCompassAPI.Application.Abstraction.Services
{
    public interface IExperienceLevelService
    {
        Task<List<ExperienceLevelGetDto>> GetAllAsync();
    }
}
