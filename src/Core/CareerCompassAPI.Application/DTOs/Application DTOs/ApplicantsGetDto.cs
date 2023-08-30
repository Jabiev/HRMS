using CareerCompassAPI.Domain.Enums;

namespace CareerCompassAPI.Application.DTOs.Application_DTOs
{
    public record ApplicantsGetDto(Guid applicationid, string firstName, string lastName, string jobTitle, string resumeLink, ApplicationStatus status);
}
