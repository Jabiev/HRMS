using CareerCompassAPI.Domain.Entities.Common;
using CareerCompassAPI.Domain.Identity;

namespace CareerCompassAPI.Domain.Entities
{
    public class Event:BaseEntity
    {
        public AppUser User { get; set; }
        public string Title { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string Importance { get; set; }
    }
}
