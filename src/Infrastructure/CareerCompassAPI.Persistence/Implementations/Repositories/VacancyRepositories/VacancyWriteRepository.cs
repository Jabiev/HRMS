using CareerCompassAPI.Application.Abstraction.Repositories.IVacancyRepositories;
using CareerCompassAPI.Domain.Entities;
using CareerCompassAPI.Persistence.Contexts;

namespace CareerCompassAPI.Persistence.Implementations.Repositories.VacancyRepositories
{
    public class VacancyWriteRepository : WriteRepository<Vacancy>, IVacancyWriteRepository
    {
        public VacancyWriteRepository(CareerCompassDbContext context) : base(context)
        {
        }
    }
}
