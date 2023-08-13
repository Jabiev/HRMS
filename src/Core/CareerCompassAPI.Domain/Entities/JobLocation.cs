namespace CareerCompassAPI.Domain.Entities
{
    public class JobLocation
    {
        public Guid Id { get; set; }
        public string JobLocationType { get; set; }
        public ICollection<Vacancy> Vacancies { get; set; }
        public string Description { get; set; }
    }
}
