namespace CareerCompassAPI.Domain.Entities
{
    public class JobType
    {
        public Guid Id { get; set; }
        public string TypeName { get; set; }
        public ICollection<Vacancy> Vacancies { get; set; }
    }
}
