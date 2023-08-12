using CareerCompassAPI.Application.Abstraction.Repositories.IIndustryRepositories;
using CareerCompassAPI.Application.Abstraction.Services;
using CareerCompassAPI.Application.DTOs.Industry_DTOs;
using CareerCompassAPI.Domain.Entities;

namespace CareerCompassAPI.Persistence.Implementations.Services
{
    public class IndustryService : IIndustryService
    {
        private readonly IIndustryWriteRepository _industryWriteRepository;

        public IndustryService(IIndustryWriteRepository industryWriteRepository)
        {
            _industryWriteRepository = industryWriteRepository;
        }

        public async Task CreateAsync(IndustryCreateDto industryCreateDto)
        {
            if (industryCreateDto is null)
            {
                throw new ArgumentNullException();
            }
            Industry newIndustry = new()
            {
                Name = industryCreateDto.industryName
            };
            await _industryWriteRepository.AddAsync(newIndustry);
            await _industryWriteRepository.SaveChangesAsync();
        }
    }
}
