using CareerCompassAPI.Domain.Entities;
using CareerCompassAPI.Domain.Identity;
using Microsoft.EntityFrameworkCore;

namespace CareerCompassAPI.Persistence.Contexts
{
    public interface ICareerCompassDbContext : IDisposable
    {
        DbSet<Subscriptions> Subscriptions { get; set; }
        DbSet<JobSeeker> JobSeekers { get; set; }
        DbSet<Recruiter> Recruiters { get; set; }
        DbSet<Company> Companies { get; set; }
        DbSet<Industry> Industries { get; set; }
        DbSet<CompanyDetails> CompanyDetails { get; set; }
        DbSet<JobType> JobTypes { get; set; }
        DbSet<ExperienceLevel> ExperienceLevels { get; set; }
        DbSet<ShiftAndSchedule> ShiftAndSchedules { get; set; }
        DbSet<JobLocation> JobLocations { get; set; }
        DbSet<Notification> Notifications { get; set; }
        DbSet<Vacancy> Vacancy { get; set; }
        DbSet<JobApplications> Applications { get; set; }
        DbSet<EducationLevel> EducationLevels { get; set; }
        DbSet<JobSeekerDetails> JobSeekerDetails { get; set; }
        DbSet<Payments> Payments { get; set; }
        DbSet<Review> Reviews { get; set; }
        DbSet<Follower> Followers { get; set; }
        DbSet<AppUser> Users { get; set; }
        DbSet<Domain.Entities.Message> Messages { get; set; }
        DbSet<Domain.Entities.File> Files { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
