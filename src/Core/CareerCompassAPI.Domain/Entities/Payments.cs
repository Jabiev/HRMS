using CareerCompassAPI.Domain.Entities.Common;
using CareerCompassAPI.Domain.Enums;
using CareerCompassAPI.Domain.Identity;

namespace CareerCompassAPI.Domain.Entities
{
    public class Payments:BaseEntity
    {
        public AppUser AppUser { get; set; } = null!;
        public long Amount { get; set; }
        public PaymentTypes Type { get; set; }
    }
}
