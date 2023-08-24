using CareerCompassAPI.Application.DTOs.Payment_DTOs;

namespace CareerCompassAPI.Application.Abstraction.Services
{
    public interface IPaymentsService
    {
        Task CreateAsync(PaymentCreateDto paymentCreateDto);
    }
}
