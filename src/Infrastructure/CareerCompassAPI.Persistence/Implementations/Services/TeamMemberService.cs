using CareerCompassAPI.Application.Abstraction.Repositories.ITeamRepositories;
using CareerCompassAPI.Application.Abstraction.Services;
using CareerCompassAPI.Application.DTOs.TeamMember_DTOs;
using CareerCompassAPI.Persistence.Exceptions;
using Microsoft.EntityFrameworkCore;

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

        public async Task<List<TeamMembersGetDto>> GetMembers()
        {
            var membersFromRepo = await _teamReadRepository.GetAllByExpression(
                m => m.IsDeleted == false,
                9,  
                0   
            ).ToListAsync();

            if (!membersFromRepo.Any())
            {
                throw new NotFoundException("Team Members do not exist");
            }

            List<TeamMembersGetDto> teamMembers = membersFromRepo.Select(member =>
                new TeamMembersGetDto(
                    member.Id,
                    member.FirstName,
                    member.LastName,
                    member.Position,
                    member.Description,
                    member.ImageUrl
                )
            ).ToList();

            return teamMembers;
        }


    }
}
