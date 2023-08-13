using CareerCompassAPI.Domain.Entities.Common;

namespace CareerCompassAPI.Domain.Entities
{
    public class Vacancy:BaseEntity
    {
        public ExperienceLevel ExperienceLevel { get; set; }
        public Recruiter Recruiter { get; set; }
        public decimal Salary { get; set; }
        public JobType JobType { get; set; }
        public ICollection<ShiftAndScheduleTag> ShiftAndScheduleTags { get; set; }
    }
}
