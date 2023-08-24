using CareerCompassAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CareerCompassAPI.Persistence.Configurations
{
    public class JobApplicationConfiguration : IEntityTypeConfiguration<JobApplications>
    {
        public void Configure(EntityTypeBuilder<JobApplications> builder)
        {
            builder.HasOne(ja => ja.JobSeeker)
                .WithMany(js => js.JobApplications)
                .HasForeignKey(ja => ja.JobSeekerId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
