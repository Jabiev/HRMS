using CareerCompassAPI.Domain.Enums;

namespace CareerCompassAPI.Application.DTOs.Payment_DTOs
{
    public record PaymentsGetDto(decimal amount, PaymentTypes paymentType, DateTime date);
}
