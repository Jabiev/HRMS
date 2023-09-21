using CareerCompassAPI.Application.DTOs.Event_DTOs;

namespace CareerCompassAPI.Application.Abstraction.Services
{
    public interface IEventService
    {
        Task<Guid> Create(EventCreateDto eventCreateDto);
    }
}
