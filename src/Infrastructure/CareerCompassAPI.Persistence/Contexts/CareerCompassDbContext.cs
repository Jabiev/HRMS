using CareerCompassAPI.Domain.Entities;
using CareerCompassAPI.Domain.Entities.Common;
using CareerCompassAPI.Domain.Identity;
using CareerCompassAPI.Persistence.Configurations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CareerCompassAPI.Persistence.Contexts
{
    public class CareerCompassDbContext:IdentityDbContext<AppUser>
    {
        public CareerCompassDbContext(DbContextOptions<CareerCompassDbContext> options):base(options) { }
        public DbSet<Subscriptions> Subscriptions { get; set; }
        public DbSet<JobSeeker> JobSeekers { get; set; }
        public DbSet<Recruiter> Recruiters { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Industry> Industries { get; set; }
        public DbSet<CompanyDetails> CompanyDetails { get; set; }
        public DbSet<JobType> JobTypes { get; set; }
        public DbSet<ExperienceLevel> ExperienceLevels { get; set; }
        public DbSet<ShiftAndSchedule> ShiftAndSchedules { get; set; }
        public DbSet<JobLocation> JobLocations { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Vacancy> Vacancy { get; set; }
        public DbSet<JobApplications> Applications { get; set; }
        public DbSet<EducationLevel> EducationLevels { get; set; }
        public DbSet<JobSeekerDetails> JobSeekerDetails { get; set; }
        public DbSet<Payments> Payments { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Domain.Entities.File> Files { get; set; }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var datas = ChangeTracker.Entries<BaseEntity>();

            foreach (var data in datas)
            {
                switch (data.State)
                {
                    case EntityState.Added:
                        data.Entity.DateCreated = DateTime.UtcNow;
                        data.Entity.DateModified = DateTime.UtcNow;
                        break;

                    case EntityState.Modified:
                        data.Entity.DateModified = DateTime.UtcNow;
                        break;
                    default:
                        break;
                }
            }
            return await base.SaveChangesAsync(cancellationToken);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); 

            modelBuilder.ApplyConfiguration(new JobApplicationConfiguration());
        }

    }
}
