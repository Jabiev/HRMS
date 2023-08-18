using CareerCompassAPI.Application.DTOs.Schedule_DTOs;

namespace CareerCompassAPI.Application.Abstraction.Services
{
    public interface IShiftScheduleService
    {
        Task<List<ShiftAndScheduleGetDto>> GetAllAsync();
    }
}
