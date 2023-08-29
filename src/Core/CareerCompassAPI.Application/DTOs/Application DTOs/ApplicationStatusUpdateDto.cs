using CareerCompassAPI.Domain.Enums;

namespace CareerCompassAPI.Application.DTOs.Application_DTOs
{
    public record ApplicationStatusUpdateDto(Guid applicationId, ApplicationStatus newStatus);
}
