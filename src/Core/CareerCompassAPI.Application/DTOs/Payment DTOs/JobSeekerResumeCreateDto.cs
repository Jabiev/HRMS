namespace CareerCompassAPI.Application.DTOs.Payment_DTOs
{
    public record JobSeekerResumeCreateDto(Guid jobSeekerId, string name, decimal amount);
}
