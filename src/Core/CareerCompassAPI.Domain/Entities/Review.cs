using CareerCompassAPI.Domain.Entities.Common;
using CareerCompassAPI.Domain.Identity;

namespace CareerCompassAPI.Domain.Entities
{
    public class Review : BaseEntity
    {
        public AppUser AppUser { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Rating { get; set; }
        public Company? Company { get; set; }
        public Industry? Industry { get; set; }

    }
}
