using CareerCompassAPI.Application.DTOs.JobType_DTOs;

namespace CareerCompassAPI.Application.Abstraction.Services
{
    public interface IJobTypeService
    {
        Task<List<JobTypeGetDto>> GetAllAsync();
    }
}
