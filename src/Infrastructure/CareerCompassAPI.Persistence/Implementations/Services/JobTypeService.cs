using AutoMapper;
using CareerCompassAPI.Application.Abstraction.Services;
using CareerCompassAPI.Application.DTOs.ExperienceLevel_DTOs;
using CareerCompassAPI.Application.DTOs.JobType_DTOs;
using CareerCompassAPI.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace CareerCompassAPI.Persistence.Implementations.Services
{
    public class JobTypeService : IJobTypeService
    {
        private readonly IMapper _mapper;
        private readonly CareerCompassDbContext _context;
        public JobTypeService(IMapper mapper, CareerCompassDbContext context)
        {
            _mapper = mapper;
            _context = context;
        }
        public async Task<List<JobTypeGetDto>> GetAllAsync()
        {
            var jobTypes = await _context.JobTypes.ToListAsync();
            List<JobTypeGetDto> jobList = _mapper.Map<List<JobTypeGetDto>>(jobTypes);
            return jobList;
        }
    }
}
