using CareerCompassAPI.Domain.Entities.Common;
using CareerCompassAPI.Domain.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace CareerCompassAPI.Domain.Entities
{
    public class Recruiter : BaseEntity
    {
        [ForeignKey("AppUser")]
        public string AppUserId { get; set; } = null!;
        public AppUser AppUser { get; set; } = null!;
        [ForeignKey("Company")]
        public Guid? CompanyId { get; set; }
        public Company? Company { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        [ForeignKey(nameof(JobLocation))]
        public Guid? JobLocationId { get; set; }
        public JobLocation? Location { get; set; }
        public Subscriptions Subscription { get; set; } = null!;
    }
}
