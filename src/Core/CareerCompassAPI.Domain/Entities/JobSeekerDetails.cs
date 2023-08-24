using CareerCompassAPI.Domain.Entities.Common;
using CareerCompassAPI.Domain.Enums;

namespace CareerCompassAPI.Domain.Entities
{
    public class JobSeekerDetails:BaseEntity
    {
        public JobSeeker JobSeeker { get; set; } = null!;
        public string? PhoneNumber { get; set; }
        public Guid EducationLevelId { get; set; }  
        public EducationLevel EducationLevel { get; set; }
        public YearsOfExperience Experience { get; set; }
        public string Description { get; set; }
    }
}
