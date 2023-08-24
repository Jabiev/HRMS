using CareerCompassAPI.Domain.Entities.Common;
using CareerCompassAPI.Domain.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace CareerCompassAPI.Domain.Entities
{
    public class JobSeeker:BaseEntity
    {
        [ForeignKey("AppUser")]
        public string AppUserId { get; set; } = null!;
        public AppUser AppUser { get; set; } = null!;
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Location { get; set; }
        public override bool IsDeleted { get; set; }
        public ICollection<JobApplications> JobApplications { get; set; }
    }
}
