using CareerCompassAPI.Application.DTOs.Industry_DTOs;

namespace CareerCompassAPI.Application.Abstraction.Services
{
    public interface IIndustryService
    {
        Task CreateAsync(IndustryCreateDto industryCreateDto);
        Task<List<IndustryGetDto>> GetAllAsync();
        Task Remove(Guid industryId);
    }
}
