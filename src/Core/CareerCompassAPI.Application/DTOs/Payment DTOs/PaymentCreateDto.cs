using CareerCompassAPI.Domain.Enums;

namespace CareerCompassAPI.Application.DTOs.Payment_DTOs
{
    public record PaymentCreateDto(string appUserId, decimal amount, PaymentTypes type);
}
