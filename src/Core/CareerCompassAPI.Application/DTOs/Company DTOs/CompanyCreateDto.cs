using CareerCompassAPI.Domain.Entities;

namespace CareerCompassAPI.Application.DTOs.Company_DTOs
{
    public record CompanyCreateDto(string name,string ceoName,DateTime dateFounded, int companySize, Guid industryId, string websiteLink, string description);
}
