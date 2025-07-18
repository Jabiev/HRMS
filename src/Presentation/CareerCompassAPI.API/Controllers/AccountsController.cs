using CareerCompassAPI.Application.Abstraction.Services;
using CareerCompassAPI.Application.Abstraction.Storage;
using CareerCompassAPI.Application.DTOs.Auth_DTOs;
using CareerCompassAPI.Application.DTOs.Password_DTOs;
using CareerCompassAPI.Application.DTOs.Response_DTOs;
using CareerCompassAPI.Domain.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CareerCompassAPI.API.Controllers;

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
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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

    [HttpPost("PasswordChange/{userId}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> PasswordChange(string userId, [FromBody] PasswordChangeDto passwordChangeDto)
    {
        var result = await _authService.PasswordChange(userId, passwordChangeDto);

        if (result)
        {
            return Ok("Password changed successfully");
        }
        else
        {
            return BadRequest("Something went wrong");
        }
    }

    [HttpGet("[action]")]
    public async Task<IActionResult> ConfirmEmail([FromQuery] string userId,
       [FromQuery] string token)
    {
        await _authService.ConfirmEmailAsync(userId, token);
        return Content(GetEmailConfirmedHtml(), "text/html");
    }

    #region Private

    private string GetEmailConfirmedHtml()
    {
        return @"<!DOCTYPE html>
        <html lang=""en"">
        <head>
          <meta charset=""UTF-8"" />
          <meta name=""viewport"" content=""width=device-width, initial-scale=1.0""/>
          <title>Email Confirmed</title>
          <link rel=""icon"" href=""https://cdn.jsdelivr.net/gh/twitter/twemoji@14.0.2/assets/svg/2705.svg"" type=""image/svg+xml"">
          <style>
            body {
              margin: 0;
              font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
              background: linear-gradient(to bottom, #0f2027, #203a43, #2c5364);
              color: white;
              display: flex;
              flex-direction: column;
              align-items: center;
              justify-content: center;
              height: 100vh;
              overflow: hidden;
              text-align: center;
            }

            .logo {
              max-width: 150px;
              margin-bottom: 20px;
            }

            .card {
              background: rgba(255, 255, 255, 0.05);
              padding: 40px;
              border-radius: 16px;
              box-shadow: 0 8px 20px rgba(0, 0, 0, 0.4);
              backdrop-filter: blur(12px);
              max-width: 400px;
            }

            .card h1 {
              font-size: 2em;
              margin-bottom: 10px;
              color: #ffcc00;
            }

            .card p {
              font-size: 1.1em;
              color: #ddd;
            }

            .success-icon {
              font-size: 4em;
              color: #00ff99;
              margin-bottom: 20px;
              animation: pop 0.6s ease-out;
            }

            @keyframes pop {
              0% { transform: scale(0); opacity: 0; }
              60% { transform: scale(1.2); opacity: 1; }
              100% { transform: scale(1); }
            }

            .footer {
              position: absolute;
              bottom: 20px;
              font-size: 0.9em;
              color: #aaa;
            }

            @media (max-width: 500px) {
              .card {
                margin: 0 20px;
                padding: 30px 20px;
              }
            }
          </style>
        </head>
        <body>
          <div class=""card"">
            <div class=""success-icon"">✅</div>
            <h1>Email Confirmed!</h1>
            <p>Thank you for verifying your email.<br/>You can now begin!</p>
          </div>

          <div class=""footer"">© 2025 by 𝕵 𝐀 𝕭 𝐈 𝕰 𝐕 and L E O. All rights reserved.</div>
        </body>
        </html>";
    }

    #endregion
}
