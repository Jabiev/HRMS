using CareerCompassAPI.Domain.Enums;

namespace CareerCompassAPI.Application.DTOs.Company_DTOs
{
    public record CompanyGetDto(Guid companyId,string name, string ceoName, int dateFounded, CompanySizeEnum companySize, string industry, string webLink, string description, string address,string logoUrl);
}
