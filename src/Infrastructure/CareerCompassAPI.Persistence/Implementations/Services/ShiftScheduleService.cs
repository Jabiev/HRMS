using AutoMapper;
using CareerCompassAPI.Application.Abstraction.Services;
using CareerCompassAPI.Application.DTOs.Industry_DTOs;
using CareerCompassAPI.Application.DTOs.Schedule_DTOs;
using CareerCompassAPI.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace CareerCompassAPI.Persistence.Implementations.Services
{
    public class ShiftScheduleService : IShiftScheduleService
    {
        private readonly IMapper _mapper;
        private readonly CareerCompassDbContext _context;

        public ShiftScheduleService(IMapper mapper, CareerCompassDbContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<List<ShiftAndScheduleGetDto>> GetAllAsync()
        {
            var schedules = await _context.ShiftAndSchedules.ToListAsync();
            List<ShiftAndScheduleGetDto> schedulesList = _mapper.Map<List<ShiftAndScheduleGetDto>>(schedules);
            return schedulesList;
        }
    }
}
