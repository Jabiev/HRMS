using CareerCompassAPI.Domain.Entities.Common;

namespace CareerCompassAPI.Domain.Entities
{
    public class CompanyDetails:BaseEntity
    {
        public string? Ceo { get; set; }
        public DateTime DateFounded { get; set; }
        public int CompanySize { get; set; }
        public Industry Industry { get; set; } = null!; 
        public string? Link { get; set; }
        public string? Description { get; set; }
    }
}
