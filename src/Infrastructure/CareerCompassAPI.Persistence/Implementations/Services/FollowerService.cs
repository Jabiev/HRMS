using CareerCompassAPI.Application.Abstraction.Repositories.ICompanyRepositories;
using CareerCompassAPI.Application.Abstraction.Repositories.IFollowerRepositories;
using CareerCompassAPI.Application.Abstraction.Services;
using CareerCompassAPI.Application.DTOs.Follower_DTOs;
using CareerCompassAPI.Domain.Entities;
using CareerCompassAPI.Domain.Identity;
using CareerCompassAPI.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace CareerCompassAPI.Persistence.Implementations.Services
{
    public class FollowerService : IFollowerService
    {
        private readonly CareerCompassDbContext _context;
        private readonly ICompanyReadRepository _companyReadRepository;
        private readonly IFollowerReadRepository _followerReadRepository;
        private readonly IFollowerWriteRepository _followerWriteRepository;

        public FollowerService(CareerCompassDbContext context,
                               ICompanyReadRepository companyReadRepository,
                               IFollowerWriteRepository followerWriteRepository,
                               IFollowerReadRepository followerReadRepository)
        {
            _context = context;
            _companyReadRepository = companyReadRepository;
            _followerWriteRepository = followerWriteRepository;
            _followerReadRepository = followerReadRepository;
        }

        public async Task CreateAsync(FollowerCreateDto followerCreateDto)
        {
            if (followerCreateDto is null)
            {
                throw new ArgumentNullException();
            }
            AppUser user = await _context.Users.FirstOrDefaultAsync(u => u.Id == followerCreateDto.appUserId);
            if (user == null) { throw new ArgumentNullException(); }
            Company company = await _companyReadRepository.GetByIdAsync(followerCreateDto.companyId);
            if (company == null) { throw new ArgumentNullException(); }
            var existingFollower = await _context.Followers
            .FirstOrDefaultAsync(f => f.User == user && f.Company == company);

            if (existingFollower != null)
            {
                throw new InvalidOperationException("User is already following this company.");
            }
            Follower newFollower = new()
            {
                User = user,
                Company = company,
            };
            await _followerWriteRepository.AddAsync(newFollower);
            await _followerWriteRepository.SaveChangesAsync();
        }

        public async Task<List<FollowerGetFollowedCompaniesDto>> GetFollowedCompanies(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentNullException(nameof(userId));
            }

            var followersQuery = _followerReadRepository.GetAllByExpression(f => f.User.Id == userId, take: 10000, skip: 0);

            if (!followersQuery.Any())
            {
                return new List<FollowerGetFollowedCompaniesDto>();
            }
            var followedCompanies = followersQuery.Select(f => new FollowerGetFollowedCompaniesDto(f.Company.Id)).ToList();
            return await Task.FromResult(followedCompanies);
        }


        public async Task Remove(FollowerRemoveDto followerRemoveDto)
        {
            if (followerRemoveDto is null)
            {
                throw new ArgumentNullException(nameof(followerRemoveDto));
            }
            AppUser user = await _context.Users.FirstOrDefaultAsync(u => u.Id == followerRemoveDto.appUserId);
            Company company = await _companyReadRepository.GetByIdAsync(followerRemoveDto.companyId);
            var existingFollower = await _context.Followers
                .FirstOrDefaultAsync(f => f.User == user && f.Company == company);

            if (existingFollower == null)
            {
                throw new ArgumentException("Follower relationship not found.");
            }

            _followerWriteRepository.Remove(existingFollower);
            await _followerWriteRepository.SaveChangesAsync();
        }
    }
}
