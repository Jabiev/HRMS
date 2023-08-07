using CareerCompassAPI.Domain.Entities.Common;
using CareerCompassAPI.Domain.Identity;

namespace CareerCompassAPI.Domain.Entities
{
    public class Subscriptions:BaseEntity
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int PostLimit { get; set; }
        public ICollection<AppUser> Users { get; set; }
    }
}
