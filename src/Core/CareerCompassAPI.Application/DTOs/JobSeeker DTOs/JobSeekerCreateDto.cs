using CareerCompassAPI.Domain.Enums;

namespace CareerCompassAPI.Application.DTOs.JobSeeker_DTOs
{
    public record JobSeekerCreateDto(string? phoneNumber, YearsOfExperience experience, Guid educationLevelId, string description);
}
