using CareerCompassAPI.Domain.Entities.Common;
using CareerCompassAPI.Domain.Identity;
using System;

namespace CareerCompassAPI.Domain.Entities
{
    public class Message : BaseEntity
    {
        public AppUser Sender { get; set; }
        public AppUser Receiver { get; set; }
        public string Content { get; set; }
        public bool IsRead { get; set; }
        public DateTime? ReadDate { get; set; }
        public string MessageType { get; set; }
    }
}
