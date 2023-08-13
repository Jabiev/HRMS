namespace CareerCompassAPI.Domain.Entities
{
    public class JobType
    {
        public int Id { get; set; }
        public string TypeName { get; set; }
        public ICollection<Vacancy> Vacancies { get; set; }
    }
}
