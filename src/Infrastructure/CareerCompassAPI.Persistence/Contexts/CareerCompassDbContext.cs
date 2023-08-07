using CareerCompassAPI.Domain.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CareerCompassAPI.Persistence.Contexts
{
    public class CareerCompassDbContext:IdentityDbContext<AppUser>
    {
        public CareerCompassDbContext(DbContextOptions<CareerCompassDbContext> options):base(options) { }
    }
}
