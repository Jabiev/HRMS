using CareerCompassAPI.Domain.Entities.Common;
using Microsoft.EntityFrameworkCore;

namespace CareerCompassAPI.Application.Abstraction.Repositories
{
    public interface IRepository<T> where T : BaseEntity, new()
    {
        public DbSet<T> Table { get; }
    }
}
