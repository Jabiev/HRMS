using CareerCompassAPI.Application.DTOs.Location_DTOs;

namespace CareerCompassAPI.Application.Abstraction.Services
{
    public interface ILocationService
    {
        Task<List<LocationGetDto>> GetAll();
        Task<List<LocationGetDto>> GetBySearch(string search);
    }
}
