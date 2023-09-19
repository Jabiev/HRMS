using CareerCompassAPI.Domain.Entities.Common;

namespace CareerCompassAPI.Domain.Entities
{
    public class Subscriptions:BaseEntity
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int PostLimit { get; set; }
        public bool isPlannerAvailable { get; set; }
        public bool isVideoAvailable { get; set; }
        public bool isChatAvailable { get; set; }
        public ICollection<Recruiter> Recruiters { get; set; }
    }
}
