using CareerCompassAPI.Application.Abstraction.Services;
using CareerCompassAPI.Application.DTOs.Response_DTOs;
using CareerCompassAPI.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace CareerCompassAPI.Infrastructure.Services.Token
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<AppUser> _userManager;
        public JwtService(IConfiguration configuration, UserManager<AppUser> userManager)
        {
            _configuration = configuration;
            _userManager = userManager;
        }
        public async Task<TokenResponseDto> CreateJwtToken(AppUser user)
        {
            DateTime jwtExpiration = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["Jwt:EXPIRATION_MINUTES"]));
            var roles = await _userManager.GetRolesAsync(user);

            List<Claim> claims = new List<Claim>
    {
        new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()), // subject user id
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // new JWT ID
        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()), // date time token generated
        new Claim(ClaimTypes.NameIdentifier, user.Email.ToString()), // Unique name identifier of the user (email)
    };

            // Add a claim for each role
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:key"]));
            SigningCredentials signingCredentials = new(securityKey, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken tokenGenerator = new JwtSecurityToken(
                _configuration["Jwt:issuer"],
                _configuration["Jwt:audience"],
                claims,
                notBefore: DateTime.UtcNow,
                expires: jwtExpiration,
                signingCredentials: signingCredentials);

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            string token = handler.WriteToken(tokenGenerator);

            string refreshToken = GenerateRefeshToken();

            // add expiration 
            DateTime refreshTokenExpiration = DateTime.Now.AddMinutes(Convert.ToInt32(_configuration["RefreshToken:EXPIRATION_MINUTES"]));
            return new TokenResponseDto(token, jwtExpiration, refreshToken, refreshTokenExpiration);
        }


        private string GenerateRefeshToken()
        {
            byte[] bytes = new byte[64];
            var randomNumber = RandomNumberGenerator.Create();
            randomNumber.GetBytes(bytes);
            return Convert.ToBase64String(bytes);
        }
    }
}
