using CareerCompassAPI.Application.DTOs.Password_DTOs;

namespace CareerCompassAPI.Application.Abstraction.Services
{
    public interface IMailService
    {
        Task SendEmailAsync(Message message);
    }
}
