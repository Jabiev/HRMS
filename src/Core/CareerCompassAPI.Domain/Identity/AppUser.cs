using CareerCompassAPI.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace CareerCompassAPI.Domain.Identity
{
    public class AppUser:IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiration { get; set; }
        public int MyProperty { get; set; }
        public Subscriptions Subscription { get; set; }
        public JobSeekers JobSeekers { get; set; }
    }  
}
