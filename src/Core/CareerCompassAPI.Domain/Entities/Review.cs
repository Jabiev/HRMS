using CareerCompassAPI.Domain.Entities.Common;
using CareerCompassAPI.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace CareerCompassAPI.Domain.Entities
{
    public class Review : BaseEntity
    {
        public JobSeeker JobSeeker { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Rating { get; set; }
        public Company? Company { get; set; }
        public ReviewStatus Status { get; set; }
    }
}
