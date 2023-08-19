using CareerCompassAPI.Domain.Entities;
using System;
using System.Collections.Generic;

namespace CareerCompassAPI.Application.DTOs.Vacancy_DTOs
{
    public record VacancyCreateDto(string jobTitle,Guid experienceLevelId,decimal salary,ICollection<Guid> jobTypeIds,Guid locationId,string description,ICollection<Guid> shiftIds
    );
}
