using CareerCompassAPI.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace CareerCompassAPI.Domain.Identity
{
    public class AppUser : IdentityUser
    {
        public AppUser()
        {
            Notifications = new HashSet<Notification>();
        }

        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiration { get; set; }
        public JobSeeker JobSeekers { get; set; }
        public Recruiter Recruiters { get; set; }
        public ICollection<Notification> Notifications { get; set; }
    }
}
