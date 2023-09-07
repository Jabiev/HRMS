using CareerCompassAPI.Domain.Entities.Common;

namespace CareerCompassAPI.Domain.Entities
{
    public class Company : BaseEntity
    {
        public string Name { get; set; } = null!;
        public string? LogoUrl { get; set; }
        public ICollection<Vacancy>? Vacancies { get; set; } = new List<Vacancy>();
        public CompanyDetails? Details { get; set; }
        public ICollection<Review>? Reviews { get; set; } = new List<Review>();
    }
}
