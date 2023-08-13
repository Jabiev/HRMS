using CareerCompassAPI.Domain.Entities.Common;

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
        public JobLocation JobLocation { get; set; }
    }
}
