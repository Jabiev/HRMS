namespace CareerCompassAPI.Application.DTOs.Application_DTOs
{
    public record ApprovedApplicantGetDto(Guid applicationId, string firstName, string lastName, string jobTitle);
}
