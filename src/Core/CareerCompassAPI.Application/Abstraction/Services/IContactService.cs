using CareerCompassAPI.Application.DTOs.Contact_DTOs;

namespace CareerCompassAPI.Application.Abstraction.Services
{
    public interface IContactService
    {
        Task SendContactEmail(ContactCreateDto contactCreateDto);
    }
}
