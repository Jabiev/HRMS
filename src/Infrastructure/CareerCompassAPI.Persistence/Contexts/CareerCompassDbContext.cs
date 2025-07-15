using CareerCompassAPI.Domain.Entities;
using CareerCompassAPI.Domain.Identity;
using CareerCompassAPI.Persistence.Configurations;
using CareerCompassAPI.Persistence.Interceptors;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using File = CareerCompassAPI.Domain.Entities.File;
using Payments = CareerCompassAPI.Domain.Entities.Payments;
using TestimonialFeedback = CareerCompassAPI.Domain.Entities.TestimonialFeedback;

namespace CareerCompassAPI.Persistence.Contexts
{
    public class CareerCompassDbContext : IdentityDbContext<AppUser>, ICareerCompassDbContext
    {
        private readonly AuditInterceptor _auditInterceptor;
        public CareerCompassDbContext() : base()
        {
        }
        public CareerCompassDbContext(DbContextOptions<CareerCompassDbContext> options) : base(options) { }
        public CareerCompassDbContext(DbContextOptions<CareerCompassDbContext> options, AuditInterceptor auditInterceptor) : base(options)
        {
            _auditInterceptor = auditInterceptor;
        }
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
        public DbSet<Resume> Resumes { get; set; }
        public DbSet<Domain.Entities.JobApplications> Applications { get; set; }
        public DbSet<EducationLevel> EducationLevels { get; set; }
        public DbSet<JobSeekerDetails> JobSeekerDetails { get; set; }
        public DbSet<Payments> Payments { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Follower> Followers { get; set; }
        public DbSet<TeamMember> Members { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<TestimonialFeedback> TestimonialFeedbacks { get; set; }
        public DbSet<Domain.Entities.Message> Messages { get; set; }
        public DbSet<Domain.Entities.File> Files { get; set; }
        public DbSet<Domain.Entities.AppSetting> Settings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<File>()
                .HasOne(f => f.User)
                .WithMany(u => u.Files)   
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Payments>()
                .HasOne(p => p.AppUser)
                .WithMany(u => u.Payments)
                .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<Domain.Entities.Message>()
                .HasOne(m => m.Sender)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Domain.Entities.Message>()
                .HasOne(m => m.Receiver)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Follower>()
               .HasOne(f => f.User)
               .WithMany()  
               .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.ApplyConfiguration(new JobApplicationConfiguration());
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.AddInterceptors(_auditInterceptor);
        }
    }
}
