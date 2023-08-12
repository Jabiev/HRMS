using CareerCompassAPI.Application.Abstraction.Repositories.IIndustryRepositories;
using CareerCompassAPI.Domain.Entities;
using CareerCompassAPI.Persistence.Contexts;

namespace CareerCompassAPI.Persistence.Implementations.Repositories.IndustryRepositories
{
    public class IndustryReadRepository : ReadRepository<Industry>, IIndustryReadRepository
    {
        public IndustryReadRepository(CareerCompassDbContext context) : base(context)
        {
        }
    }
}
