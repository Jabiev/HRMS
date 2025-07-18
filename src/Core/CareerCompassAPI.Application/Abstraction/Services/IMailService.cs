using CareerCompassAPI.Application.DTOs.Password_DTOs;
using CareerCompassAPI.Domain.Identity;

namespace CareerCompassAPI.Application.Abstraction.Services
{
    public interface IMailService
    {
        Task SendEmailConfirmationAsync(AppUser user, string confirmationLink);
        Task SendEmailAsync(Message message);
    }
}
