using CareerCompassAPI.Domain.Enums;

namespace CareerCompassAPI.Application.DTOs.Company_DTOs
{
    public record CompanyCreateDto(string name, string ceoName, int dateFounded, string address,CompanySizeEnum companySize, Guid industryId, Guid locationId,string websiteLink, string description);
}
