using CareerCompassAPI.Domain.Entities.Common;

namespace CareerCompassAPI.Domain.Entities
{
    public class Testimonials:BaseEntity
    {
        public string ImagePath { get; set; }
        public JobSeeker JobSeeker { get; set; }
    }
}
