using CareerCompassAPI.Domain.Entities;
using CareerCompassAPI.Domain.Entities.Common;
using CareerCompassAPI.Domain.Identity;
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
    }
}
