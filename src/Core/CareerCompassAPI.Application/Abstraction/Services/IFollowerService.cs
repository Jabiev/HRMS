using CareerCompassAPI.Application.DTOs.Follower_DTOs;

namespace CareerCompassAPI.Application.Abstraction.Services
{
    public interface IFollowerService
    {
        Task CreateAsync(FollowerCreateDto followerCreateDto);
        Task Remove(FollowerRemoveDto followerRemoveDto);
        Task<List<FollowerGetFollowedCompaniesDto>> GetFollowedCompanies(string userId);
        Task<List<GetAllFollowersDto>> GetAllFollowersByCompanyId(Guid companyId);
    }
}
