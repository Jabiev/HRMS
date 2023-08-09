using CareerCompassAPI.Application.Abstraction.Repositories.IJobSeekerRepositories;
using CareerCompassAPI.Application.Abstraction.Repositories.ISubscriptionRepository;
using CareerCompassAPI.Application.Abstraction.Services;
using CareerCompassAPI.Application.DTOs.Auth_DTOs;
using CareerCompassAPI.Domain.Entities;
using CareerCompassAPI.Domain.Enums;
using CareerCompassAPI.Domain.Identity;
using CareerCompassAPI.Persistence.Exceptions;
using Microsoft.AspNetCore.Identity;
using System.Text;

namespace CareerCompassAPI.Persistence.Implementations.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IJobSeekerWriteRepository _jobSeekerWriteRepository;
        private readonly ISubscriptionReadRepository _subscriptionReadRepository;
        public AuthService(UserManager<AppUser> userManager,
                           IJobSeekerWriteRepository jobSeekerWriteRepository,
                           ISubscriptionReadRepository subscriptionReadRepository)
        {
            _userManager = userManager;
            _jobSeekerWriteRepository = jobSeekerWriteRepository;
            _subscriptionReadRepository = subscriptionReadRepository;
        }
        public async Task Register(UserRegisterDto userRegisterDto)
        {
            var freeSubscription = await _subscriptionReadRepository.GetByExpressionAsync(s => s.Name == "Free");
            AppUser appUser = new()
            {
                UserName = userRegisterDto.email,
                FirstName = userRegisterDto.firstName,
                LastName = userRegisterDto.lastName,
                Email = userRegisterDto.email,
                Subscription = freeSubscription
            };
            IdentityResult identityResult = await _userManager.CreateAsync(appUser,userRegisterDto.password);
            
            if (!identityResult.Succeeded)
            {
                StringBuilder errorMessage = new();
                foreach (var error in identityResult.Errors)
                {
                    errorMessage.AppendLine(error.Description);
                }
                throw new UserRegistrationException(errorMessage.ToString());
            }
            var result = await _userManager.AddToRoleAsync(appUser, userRegisterDto.role.ToString());
            if (!result.Succeeded)
            {
                StringBuilder errorMessage = new();
                foreach (var error in identityResult.Errors)
                {
                    errorMessage.AppendLine(error.Description);
                }
                throw new UserRegistrationException(errorMessage.ToString());
            }
            if (userRegisterDto.role == Roles.JobSeeker)
            {
                JobSeekers jobSeeker = new()
                {
                    AppUserId = appUser.Id,
                    FirstName = userRegisterDto.firstName,
                    LastName = userRegisterDto.lastName,
                    Email = userRegisterDto.email,
                };
                await _jobSeekerWriteRepository.AddAsync(jobSeeker);
                await _jobSeekerWriteRepository.SaveChangesAsync();
            }
        }
    }
}
