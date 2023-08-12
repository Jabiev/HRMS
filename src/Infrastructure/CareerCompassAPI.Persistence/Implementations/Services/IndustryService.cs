using AutoMapper;
using CareerCompassAPI.Application.Abstraction.Repositories.IIndustryRepositories;
using CareerCompassAPI.Application.Abstraction.Services;
using CareerCompassAPI.Application.DTOs.Industry_DTOs;
using CareerCompassAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CareerCompassAPI.Persistence.Implementations.Services
{
    public class IndustryService : IIndustryService
    {
        private readonly IIndustryWriteRepository _industryWriteRepository;
        private readonly IMapper _mapper;
        private readonly IIndustryReadRepository _industryReadRepository;

        public IndustryService(IIndustryWriteRepository industryWriteRepository,
                               IMapper mapper,
                               IIndustryReadRepository industryReadRepository)
        {
            _industryWriteRepository = industryWriteRepository;
            _mapper = mapper;
            _industryReadRepository = industryReadRepository;
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

        public async Task<List<IndustryGetDto>> GetAllAsync()
        {
            var industries = await _industryReadRepository.GetAll().ToListAsync();
            List<IndustryGetDto> industryList = _mapper.Map<List<IndustryGetDto>>(industries);
            return industryList;
        }

        public async Task Remove(Guid industryId)
        {
            var industry = await _industryReadRepository.GetByIdAsync(industryId);
            if (industry is not Industry)
            {
                throw new ArgumentNullException();
            }
            _industryWriteRepository.Remove(industry);
            await _industryWriteRepository.SaveChangesAsync();
        }
    }
}
