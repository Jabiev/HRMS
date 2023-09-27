namespace CareerCompassAPI.Application.DTOs.Vacancy_DTOs
{
    public record VacancyUpdateDto(Guid id, string jobTitle, string description, decimal salary);
}
