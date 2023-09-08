using CareerCompassAPI.Domain.Entities.Common;
using CareerCompassAPI.Domain.Identity;

namespace CareerCompassAPI.Domain.Entities
{
    public class Follower:BaseEntity
    {
        public AppUser User { get; set; }
        public Company Company { get; set; }
    }
}
