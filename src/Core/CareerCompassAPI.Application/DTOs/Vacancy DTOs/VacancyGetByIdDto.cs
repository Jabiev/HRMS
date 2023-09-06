namespace CareerCompassAPI.Application.DTOs.Vacancy_DTOs
{
    public record VacancyGetByIdDto(Guid id,string jobTitle, string companyName, string jobLocation);
}
