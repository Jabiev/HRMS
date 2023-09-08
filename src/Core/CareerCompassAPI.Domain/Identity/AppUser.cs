using CareerCompassAPI.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using File = CareerCompassAPI.Domain.Entities.File;

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
        public ICollection<Payments> Payments { get; set; }
        public ICollection<File> Files { get; set; }
        public ICollection<Follower> Followers { get; set; } = new List<Follower>();
    }
}
