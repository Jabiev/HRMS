using CareerCompassAPI.Application.Abstraction.Repositories.ITeamRepositories;
using CareerCompassAPI.Application.Abstraction.Services;
using CareerCompassAPI.Application.DTOs.TeamMember_DTOs;

namespace CareerCompassAPI.Services
{
    public class TeamMemberService:ITeamMemberService
    {
        private readonly ITeamReadRepository _teamReadRepository; 

        public TeamMemberService(ITeamReadRepository teamReadRepository)
        {
            _teamReadRepository = teamReadRepository;
        }

        public Task CreateMember(TeamMemberCreateDto teamMemberCreateDto)
        {
            throw new NotImplementedException();
        }

        public Task<List<TeamMembersGetDto>> GetMembers()
        {
            var membersFromRepo = _teamReadRepository.GetAll();

            List<TeamMembersGetDto> teamMembers = new List<TeamMembersGetDto>();

            foreach (var member in membersFromRepo)
            {
                var dto = new TeamMembersGetDto(
                    member.Id,
                    member.FirstName,
                    member.LastName,
                    member.Position,
                    member.Description,
                    member.ImageUrl
                );

                teamMembers.Add(dto);
            }

            return Task.FromResult(teamMembers);
        }

    }
}
