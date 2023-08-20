using CareerCompassAPI.Application.Abstraction.Services;
using CareerCompassAPI.Application.Abstraction.Storage;
using CareerCompassAPI.Application.DTOs.Auth_DTOs;
using CareerCompassAPI.Application.DTOs.Password_DTOs;
using CareerCompassAPI.Application.DTOs.Response_DTOs;
using CareerCompassAPI.Domain.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CareerCompassAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   
    public class AccountsController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IMailService _mailService;
        private readonly IStorageService _storageService;
        private readonly ILogger<AccountsController> _logger;
        public AccountsController(IAuthService authService,
                                  SignInManager<AppUser> signInManager,
                                  IMailService mailService,
                                  IStorageService storageService,
                                  ILogger<AccountsController> logger)
        {
            _authService = authService;
            _signInManager = signInManager;
            _mailService = mailService;
            _storageService = storageService;
            _logger = logger;
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto userRegisterDto)
        {
            await _authService.Register(userRegisterDto);
            return Ok();
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> Login(UserSignInDto signInDto)
        {
            TokenResponseDto tokenResponse = await _authService.Login(signInDto);
            return Ok(tokenResponse);
        }
        [HttpPost("[action]")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok();
        }
        [HttpGet("[action]")]
        public async Task<IActionResult> RefreshToken([FromQuery] string token)
        {
            var response = await _authService.ValidateRefreshToken(token);
            return Ok(response);
        }
        [HttpGet("[action]")]
        public async Task<IActionResult> MailTest()
        {
            var emailAddresses = new List<string> { "rhabibli@outlook.com" };
            var emailSubject = "Test Email";
            var emailContent = "Hello, this is a test email.";

            var emailMessage = new Message(emailAddresses, emailSubject, emailContent);

            await _mailService.SendEmailAsync(emailMessage);  
            return Ok();
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto forgotPasswordDto)
        {
            await _authService.ForgotPassword(forgotPasswordDto);
            return Ok();
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto resetPasswordDto, [FromQuery] string userId, [FromQuery] string token)
        {
            string correctedToken = token.Replace(" ", "+");
            await _authService.ResetPassword(resetPasswordDto, userId, correctedToken);
            return Ok("Password reset successfully");
        }

        //[HttpPost("[action]")]
        //public async Task<IActionResult> Upload()
        //{
        //     await _storageService.UploadAsync("resourse/files", Request.Form.Files);
        //}
    }
}
