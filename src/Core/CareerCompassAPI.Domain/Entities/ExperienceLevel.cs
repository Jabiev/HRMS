using CareerCompassAPI.Domain.Entities.Common;

namespace CareerCompassAPI.Domain.Entities
{
    public class ExperienceLevel
    {
        public int Id { get; set; }
        public string LevelName { get; set; }
        public ICollection<Vacancy> Vacancies { get; set; }
    }
}
