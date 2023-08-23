namespace CareerCompassAPI.Application.DTOs.Vacancy_DTOs
{
    public record VacancyCreateDto(string jobTitle,Guid experienceLevelId,decimal salary,ICollection<Guid> jobTypeIds,Guid locationId,int applicationLimit, string description,ICollection<Guid> shiftIds
    );
}
