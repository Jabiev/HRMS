using AutoMapper;
using CareerCompassAPI.Application.Abstraction.Services;
using CareerCompassAPI.Application.DTOs.Location_DTOs;
using CareerCompassAPI.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace CareerCompassAPI.Persistence.Implementations.Services
{
    public class LocationService : ILocationService
    {
        private readonly CareerCompassDbContext _context;
        private readonly IMapper _mapper;

        public LocationService(CareerCompassDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;   
        }

        public async Task<List<LocationGetDto>> GetAll()
        {
            var locations = await _context.JobLocations.ToListAsync();
            List<LocationGetDto> list = _mapper.Map<List<LocationGetDto>>(locations);
            return list;
        }
    }
}
