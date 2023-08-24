using CareerCompassAPI.Domain.Entities.Common;
using CareerCompassAPI.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace CareerCompassAPI.Domain.Entities
{
    public class JobApplications:BaseEntity
    {
        [ForeignKey(nameof(JobSeeker))]
        public Guid JobSeekerId { get; set; }
        public JobSeeker JobSeeker { get; set; } = null!;
        public Vacancy Vacancy { get; set; }=null!;
        public ApplicationStatus Status { get; set; }
        public DateTime Expiration { get; set; }
    }
}
