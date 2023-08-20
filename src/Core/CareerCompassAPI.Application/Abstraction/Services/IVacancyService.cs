using CareerCompassAPI.Application.DTOs.Vacancy_DTOs;

namespace CareerCompassAPI.Application.Abstraction.Services
{
    public interface IVacancyService
    {
        Task Create(VacancyCreateDto vacancyCreateDto,string userId, Guid companyId);
        Task<List<VacancyGetDto>> GetBySearch(string jobTitle);
    }
}
