using CareerCompassAPI.Domain.Entities.Common;

namespace CareerCompassAPI.Domain.Entities
{
    public class Industry:BaseEntity
    {
        public string Name { get; set; } = null!;
        public ICollection<Company>? Companies { get; set; }
    }
}
