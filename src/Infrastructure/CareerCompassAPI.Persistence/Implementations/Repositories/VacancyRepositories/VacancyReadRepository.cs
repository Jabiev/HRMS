using CareerCompassAPI.Application.Abstraction.Repositories.IVacancyRepositories;
using CareerCompassAPI.Domain.Entities;
using CareerCompassAPI.Persistence.Contexts;

namespace CareerCompassAPI.Persistence.Implementations.Repositories.VacancyRepositories
{
    public class VacancyReadRepository : ReadRepository<Vacancy>, IVacancyReadRepository
    {
        public VacancyReadRepository(CareerCompassDbContext context) : base(context)
        {
        }
    }
}
