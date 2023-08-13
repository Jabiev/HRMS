using CareerCompassAPI.Domain.Entities.Common;
using CareerCompassAPI.Domain.Enums;
using CareerCompassAPI.Domain.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace CareerCompassAPI.Domain.Entities
{
    public class Notification:BaseEntity
    {
        public string? Title { get; set; }
        public string Message { get; set; }
        public ReadStatus ReadStatus { get; set; }
        public virtual AppUser User { get; set; }
        [ForeignKey(nameof(AppUser))]
        public Guid UserId { get; set; }
    }
}
