using Microsoft.AspNetCore.Identity;

namespace CareerCompassAPI.Domain.Identity
{
    public class AppUser:IdentityUser
    {
        public string? Fullname { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiration { get; set; }
    }
}
