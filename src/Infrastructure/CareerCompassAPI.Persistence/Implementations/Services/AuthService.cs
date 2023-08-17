using CareerCompassAPI.Application.Abstraction.Repositories.IJobSeekerRepositories;
using CareerCompassAPI.Application.Abstraction.Repositories.IRecruiterRepositories;
using CareerCompassAPI.Application.Abstraction.Repositories.ISubscriptionRepository;
using CareerCompassAPI.Application.Abstraction.Services;
using CareerCompassAPI.Application.DTOs.Auth_DTOs;
using CareerCompassAPI.Application.DTOs.Password_DTOs;
using CareerCompassAPI.Application.DTOs.Response_DTOs;
using CareerCompassAPI.Domain.Entities;
using CareerCompassAPI.Domain.Enums;
using CareerCompassAPI.Domain.Identity;
using CareerCompassAPI.Persistence.Contexts;
using CareerCompassAPI.Persistence.Exceptions.AuthExceptions;
using Hangfire;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Web;

namespace CareerCompassAPI.Persistence.Implementations.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IJobSeekerWriteRepository _jobSeekerWriteRepository;
        private readonly ISubscriptionReadRepository _subscriptionReadRepository;
        private readonly IRecruiterWriteRepository _recruiterWriteRepository;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IJwtService _jwtService;
        private readonly IMailService _mailService;
        private readonly CareerCompassDbContext _context;
        private readonly INotificationService _notificationService;
        public AuthService(UserManager<AppUser> userManager,
                           IJobSeekerWriteRepository jobSeekerWriteRepository,
                           ISubscriptionReadRepository subscriptionReadRepository,
                           IRecruiterWriteRepository recruiterWriteRepository,
                           SignInManager<AppUser> signInManager,
                           IJwtService jwtService,
                           CareerCompassDbContext context,
                           INotificationService notificationService,
                           IMailService mailService)
        {
            _userManager = userManager;
            _jobSeekerWriteRepository = jobSeekerWriteRepository;
            _subscriptionReadRepository = subscriptionReadRepository;
            _recruiterWriteRepository = recruiterWriteRepository;
            _signInManager = signInManager;
            _jwtService = jwtService;
            _context = context;
            _notificationService = notificationService;
            _mailService = mailService;
        }

        public async Task ForgotPassword(ForgotPasswordDto forgotPasswordDto)
        {
            var user = await _userManager.FindByEmailAsync(forgotPasswordDto.email);
            if (user == null) { return; }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var callbackUrl = GenerateResetPasswordUrl(user.Id, token);

            var message = new Message(new string[] { forgotPasswordDto.email },
                                      "Reset Password",
                                      $"Please reset your password by clicking here: <a href='{callbackUrl}'>link</a>");
            await _mailService.SendEmailAsync(message);
        }

        public async Task<TokenResponseDto> Login(UserSignInDto userSignInDto)
        {
            AppUser user = await _userManager.FindByEmailAsync(userSignInDto.email);
            if (user is not AppUser)
            {
                throw new SignInFailureException("Failed to sign in");
            }
            SignInResult signInResult = await _signInManager.CheckPasswordSignInAsync(user, userSignInDto.password, true);
            if (!signInResult.Succeeded)
            {
                throw new SignInFailureException("Failed to sign in");
            }
            TokenResponseDto tokenResponse = await _jwtService.CreateJwtToken(user);
            user.RefreshToken = tokenResponse.refreshToken;
            user.RefreshTokenExpiration = tokenResponse.refreshTokenExpiration;
            await _userManager.UpdateAsync(user);
            return tokenResponse;
        }

        public async Task Register(UserRegisterDto userRegisterDto)
        {
            var freeSubscription = await _subscriptionReadRepository.GetByExpressionAsync(s => s.Price == 0);
            AppUser appUser = new()
            {
                UserName = userRegisterDto.email,
                Email = userRegisterDto.email,
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
                JobSeeker jobSeeker = new()
                {
                    AppUserId = appUser.Id,
                    FirstName = userRegisterDto.firstName,
                    LastName = userRegisterDto.lastName,
                };
                await _jobSeekerWriteRepository.AddAsync(jobSeeker);
                await _jobSeekerWriteRepository.SaveChangesAsync();
            }
            if (userRegisterDto.role == Roles.Recruiter)
            {
                Recruiter recruiter = new()
                {
                    AppUserId = appUser.Id,
                    FirstName = userRegisterDto.firstName,
                    LastName = userRegisterDto.lastName,
                    Subscription = freeSubscription
                };
                await _recruiterWriteRepository.AddAsync(recruiter);
                await _recruiterWriteRepository.SaveChangesAsync();

                var userId = Guid.Parse(appUser.Id);
                var title = "Welcome to Career Compass";
                var message = "Please register your company's name before proceeding to post any job vacancies.";
                BackgroundJob.Schedule<INotificationService>(x => x.CreateAsync(userId, title, message), TimeSpan.FromSeconds(10));
            }
        }

        public async Task ResetPassword(ResetPasswordDto resetPasswordDto, string userId, string urlEncodedToken)
        {
            var token = HttpUtility.UrlDecode(urlEncodedToken);
            if (resetPasswordDto.password != resetPasswordDto.confirmPassword)
            {
                throw new ArgumentException("Passwords do not match.");
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new InvalidOperationException("User not found.");
            }

            var result = await _userManager.ResetPasswordAsync(user, token, resetPasswordDto.password);
            if (!result.Succeeded)
            {
                StringBuilder errorMessage = new StringBuilder();
                foreach (var error in result.Errors)
                {
                    errorMessage.AppendLine(error.Description);
                }
                throw new InvalidOperationException(errorMessage.ToString());
            }
        }


        public async Task<TokenResponseDto> ValidateRefreshToken(string refreshToken)
        {
            if (refreshToken is null)
            {
                throw new ArgumentNullException("Refresh token does not exist");
            }
            AppUser? user = await _context.Users.Where(u => u.RefreshToken == refreshToken).FirstOrDefaultAsync();
            if (user is not AppUser)
            {
                throw new ArgumentNullException("User does not exist");
            }
            if (user.RefreshTokenExpiration < DateTime.UtcNow)
            {
                throw new ArgumentNullException("Refresh token does not exist");
            }
            TokenResponseDto tokenResponse = await _jwtService.CreateJwtToken(user);
            user.RefreshToken = tokenResponse.refreshToken;
            user.RefreshTokenExpiration = tokenResponse.refreshTokenExpiration;
            await _userManager.UpdateAsync(user);
            return tokenResponse;
        }
        private string GenerateResetPasswordUrl(string userId, string token)
        {
            var baseUrl = "http://localhost:3000";
            var resetPasswordPath = $"/passwordreset?userId={userId}&token={HttpUtility.UrlEncode(token)}";
            return baseUrl + resetPasswordPath;
        }
    }
}
