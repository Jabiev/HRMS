namespace CareerCompassAPI.Application.DTOs.TeamMember_DTOs
{
    public record TeamMembersGetDto(Guid memberId, string firstName,string lastName, string position, string description, string imageUrl);
}
