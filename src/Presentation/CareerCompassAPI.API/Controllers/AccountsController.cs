using CareerCompassAPI.Application.Abstraction.Services;
using CareerCompassAPI.Application.DTOs.Auth_DTOs;
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
        public AccountsController(IAuthService authService,
                                  SignInManager<AppUser> signInManager)
        {
            _authService = authService;
            _signInManager = signInManager;
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> Register([FromBody]UserRegisterDto userRegisterDto)
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
    }
}
