using CareerCompassAPI.Domain.Enums;

namespace CareerCompassAPI.Application.DTOs.Payment_DTOs
{
    public record PaymentCreateDto(string appUserId, long amount, PaymentTypes type);
}
