using CareerCompassAPI.Application.DTOs.Message_DTOs;

namespace CareerCompassAPI.Application.Abstraction.Services
{
    public interface IMessageService
    {
        Task CreateAsync(MessageCreateDto messageCreateDto);
    }
}
