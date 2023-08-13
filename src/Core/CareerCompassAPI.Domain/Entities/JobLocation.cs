namespace CareerCompassAPI.Domain.Entities
{
    public class JobLocation
    {
        public Guid Id { get; set; }
        public string Location { get; set; }
        public ICollection<Vacancy> Vacancies { get; set; }
    }
}
