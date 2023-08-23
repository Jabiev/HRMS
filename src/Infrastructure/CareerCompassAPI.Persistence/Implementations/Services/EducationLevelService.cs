using AutoMapper;
using CareerCompassAPI.Application.Abstraction.Services;
using CareerCompassAPI.Application.DTOs.EducationLevel_DTOs;
using CareerCompassAPI.Application.DTOs.ExperienceLevel_DTOs;
using CareerCompassAPI.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace CareerCompassAPI.Persistence.Implementations.Services
{
    public class EducationLevelService : IEducationLevelService
    {
        private readonly CareerCompassDbContext _context;
        private readonly IMapper _mapper;

        public EducationLevelService(CareerCompassDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<EducationLevelGetDto>> GetAllAsync()
        {
            var educationLevels = await _context.EducationLevels.ToListAsync();
            List<EducationLevelGetDto> levelsList = _mapper.Map<List<EducationLevelGetDto>>(educationLevels);
            return levelsList;
        }
    }
}
