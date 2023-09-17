using CareerCompassAPI.Application.Abstraction.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CareerCompassAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamMembersController : ControllerBase
    {
        private readonly ITeamMemberService _teamMemberService;

        public TeamMembersController(ITeamMemberService teamMemberService)
        {
            _teamMemberService = teamMemberService;
        }
        [HttpGet("[action]")]
        public async Task<IActionResult> GetMembers()
        {
            var response = await _teamMemberService.GetMembers();
            return Ok(response);
        }
    }
}
