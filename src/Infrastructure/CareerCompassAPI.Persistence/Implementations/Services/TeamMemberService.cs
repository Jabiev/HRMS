using CareerCompassAPI.Application.Abstraction.Repositories.ITeamRepositories;
using CareerCompassAPI.Application.Abstraction.Services;
using CareerCompassAPI.Application.DTOs.TeamMember_DTOs;
using CareerCompassAPI.Domain.Entities;

namespace CareerCompassAPI.Persistence.Implementations.Services
{
    public class TeamMemberService : ITeamMemberService
    {
        private readonly ITeamWriteRepository _teamWriteRepository;

        public TeamMemberService(ITeamWriteRepository teamWriteRepository)
        {
            _teamWriteRepository = teamWriteRepository;
        }

        public async Task CreateMember(TeamMemberCreateDto teamMemberCreateDto)
        {
            if (teamMemberCreateDto is null)
            {
                throw new ArgumentNullException("Parameter passed in may not include null values");
            }
            TeamMember newMember = new()
            {
                FirstName = teamMemberCreateDto.firstName,
                LastName = teamMemberCreateDto.lastName,
                Description = teamMemberCreateDto.description,
                Position = teamMemberCreateDto.position,
                ImageUrl = teamMemberCreateDto.imageUrl,
            };
            await _teamWriteRepository.AddAsync(newMember);
            await _teamWriteRepository.SaveChangesAsync();
        }
    }
}
