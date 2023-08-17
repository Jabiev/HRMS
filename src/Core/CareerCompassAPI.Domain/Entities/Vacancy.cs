using CareerCompassAPI.Domain.Entities.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace CareerCompassAPI.Domain.Entities
{
    public class Vacancy : BaseEntity
    {
        public string JobTitle { get; set; } = null!;
        public ExperienceLevel ExperienceLevel { get; set; } = null!;
        public Recruiter Recruiter { get; set; } = null!;
        public decimal Salary { get; set; }
        public JobType JobType { get; set; } = null!;
        public ICollection<ShiftAndSchedule> ShiftAndSchedules { get; set; } = null!;
        [ForeignKey(nameof(JobLocation))]
        public Guid JobLocationId { get; set; }
        public JobLocation JobLocation { get; set; }
        public string Description { get; set; } = null!;
        public Company Company { get; set; } = null!;

    }
}
