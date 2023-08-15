using CareerCompassAPI.Domain.Entities.Common;
using CareerCompassAPI.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace CareerCompassAPI.Domain.Entities
{
    public class CompanyDetails:BaseEntity
    {
        public string? Ceo { get; set; }
        public int DateFounded { get; set; }
        public CompanySizeEnum CompanySize { get; set; }
        public Industry Industry { get; set; } = null!; 
        public string? Link { get; set; }
        public string? Description { get; set; }
        public string? Address { get; set; }

        public JobLocation? Location { get; set; }
    }
}
