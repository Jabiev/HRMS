namespace CareerCompassAPI.Domain.Entities
{
    public class EducationLevel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ICollection<JobSeekerDetails> JobSeekerDetails { get; set; }
    }
}
