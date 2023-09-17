using CareerCompassAPI.Application.DTOs.TeamMember_DTOs;

namespace CareerCompassAPI.Application.Abstraction.Services
{
    public interface ITeamMemberService
    {
        Task CreateMember(TeamMemberCreateDto teamMemberCreateDto);
    }
}
