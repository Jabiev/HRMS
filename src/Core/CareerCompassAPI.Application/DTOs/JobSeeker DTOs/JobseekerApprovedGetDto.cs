namespace CareerCompassAPI.Application.DTOs.JobSeeker_DTOs
{
    public record JobseekerApprovedGetDto(Guid applicationId, string firstName, string lastName, string companyName);
}
