using CareerCompassAPI.Domain.Entities.Common;

namespace CareerCompassAPI.Domain.Entities
{
    public class ShiftAndScheduleTag
    {
        public int Id { get; set; }
        public string ShiftName { get; set; }
        public ICollection<Vacancy> Vacancies { get; set; }
    }
}
