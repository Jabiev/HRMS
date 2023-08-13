using CareerCompassAPI.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace CareerCompassAPI.Domain.Identity
{
    public class AppUser:IdentityUser
    {
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiration { get; set; }
        public JobSeeker JobSeekers { get; set; }
        public Recruiter Recruiters { get; set; }
    }  
}
