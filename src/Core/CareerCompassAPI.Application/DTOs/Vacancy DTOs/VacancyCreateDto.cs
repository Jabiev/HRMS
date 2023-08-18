using CareerCompassAPI.Domain.Entities;

namespace CareerCompassAPI.Application.DTOs.Vacancy_DTOs
{
    public record VacancyCreateDto(string jobTitle, Guid experienceLevelId, decimal salary, Guid jobTypeId, Guid jobLocationId, string description,ICollection<ShiftAndSchedule> shifts);
 }
