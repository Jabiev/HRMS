using AutoMapper;
using CareerCompassAPI.Application.Abstraction.Services;
using CareerCompassAPI.Application.DTOs.ExperienceLevel_DTOs;
using CareerCompassAPI.Application.DTOs.Industry_DTOs;
using CareerCompassAPI.Domain.Entities;
using CareerCompassAPI.Persistence.Contexts;
using CareerCompassAPI.Persistence.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace CareerCompassAPI.Persistence.Implementations.Services
{
    public class ExperienceLevelService : IExperienceLevelService
    {
        private readonly IMapper _mapper;
        private readonly CareerCompassDbContext _context;
        public ExperienceLevelService(IMapper mapper, CareerCompassDbContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<List<ExperienceLevelGetDto>> GetAllAsync()
        {
            var experienceLevels = await _context.ExperienceLevels.ToListAsync();
            if (experienceLevels is null)
            {
                throw new NotFoundException("Experience levels do not exist");
            }
            List<ExperienceLevelGetDto> experienceList = _mapper.Map<List<ExperienceLevelGetDto>>(experienceLevels);
            return experienceList;
        }
    }
}
