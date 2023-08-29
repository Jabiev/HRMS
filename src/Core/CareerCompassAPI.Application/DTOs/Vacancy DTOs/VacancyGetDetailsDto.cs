namespace CareerCompassAPI.Application.DTOs.Vacancy_DTOs
{
    public record VacancyGetDetailsDto(Guid id, string jobTitle, string companyName, string locationName, decimal salary, List<string> jobTypeIds, List<string> shiftAndScheduleIds,string description,string companyLink, DateTime dateCreated, int applicationLimit, int currentApplicationCount);
}
