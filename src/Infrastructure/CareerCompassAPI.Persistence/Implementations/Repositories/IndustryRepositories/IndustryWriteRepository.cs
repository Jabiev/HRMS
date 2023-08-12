using CareerCompassAPI.Application.Abstraction.Repositories.IIndustryRepositories;
using CareerCompassAPI.Domain.Entities;
using CareerCompassAPI.Persistence.Contexts;

namespace CareerCompassAPI.Persistence.Implementations.Repositories.IndustryRepositories
{
    public class IndustryWriteRepository : WriteRepository<Industry>, IIndustryWriteRepository
    {
        public IndustryWriteRepository(CareerCompassDbContext context) : base(context)
        {
        }
    }
}
