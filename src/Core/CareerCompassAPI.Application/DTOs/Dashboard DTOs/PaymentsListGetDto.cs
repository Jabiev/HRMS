using CareerCompassAPI.Domain.Enums;

namespace CareerCompassAPI.Application.DTOs.Dashboard_DTOs
{
    public record PaymentsListGetDto(Guid paymentId, string payersId, decimal paymentAmount, string paymentType, string dateCreated);
}
