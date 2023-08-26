using CareerCompassAPI.Domain.Entities.Common;
using CareerCompassAPI.Domain.Identity;
using System;

namespace CareerCompassAPI.Domain.Entities
{
    public class File : BaseEntity
    {
        public string Name { get; set; }
        public string BlobPath { get; set; } 
        public string ContainerName { get; set; } 
        public string ContentType { get; set; } 
        public long Size { get; set; } 
        public AppUser User { get; set; } 
    }
}
