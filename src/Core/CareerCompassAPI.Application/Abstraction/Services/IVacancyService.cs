using CareerCompassAPI.Application.DTOs.Vacancy_DTOs;
using CareerCompassAPI.Domain.Concretes;

namespace CareerCompassAPI.Application.Abstraction.Services
{
    public interface IVacancyService
    {
        Task Create(VacancyCreateDto vacancyCreateDto, string userId, Guid companyId);
        Task<List<VacancyGetDto>> GetBySearch(string jobTitle);
        Task<List<VacancyGetDetailsDto>> GetDetailsBySearch(string? jobTitle, Guid? locationId, string? sortOrder, string? jobType, decimal? minSalary,
    decimal? maxSalary);

        Task<List<VacancyGetByIdDto>> GetVacancyByRecruiterId(Guid id);
        Task DeleteVacancyById(Guid id);
        Task UpdateVacancy(VacancyUpdateDto vacancyUpdateDto);

    }
}
