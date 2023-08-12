using CareerCompassAPI.Domain.Entities;

namespace CareerCompassAPI.Application.DTOs.Company_DTOs
{
    public record CompanyCreateDto(string name,string ceoName,int dateFounded, int companySize, Guid industryId, string websiteLink, string description);
}
