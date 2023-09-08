using CareerCompassAPI.Application.DTOs.Vacancy_DTOs;

namespace CareerCompassAPI.Application.Abstraction.Services
{
    public interface IVacancyService
    {
        Task Create(VacancyCreateDto vacancyCreateDto,string userId, Guid companyId);
        Task<List<VacancyGetDto>> GetBySearch(string jobTitle);
        Task<List<VacancyGetDetailsDto>> GetDetailsBySearch(string? jobTitle, Guid? locationId, int page, int pageSize);
        Task<List<VacancyGetByIdDto>> GetVacancyByRecruiterId(Guid id);
        Task DeleteVacancyById(Guid id);
        //Task SendNewVacancyEmail();
    }
}
