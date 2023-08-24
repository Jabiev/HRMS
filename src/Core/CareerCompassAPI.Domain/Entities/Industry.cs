using CareerCompassAPI.Domain.Entities.Common;
using System.Collections.Generic;

namespace CareerCompassAPI.Domain.Entities
{
    public class Industry : BaseEntity
    {
        public string Name { get; set; } = null!;
        public ICollection<Company>? Companies { get; set; } = new List<Company>();
        public ICollection<Review>? Reviews { get; set; } = new List<Review>();
    }
}
