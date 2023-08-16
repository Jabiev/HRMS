using CareerCompassAPI.Domain.Enums;

namespace CareerCompassAPI.Application.DTOs.Company_DTOs
{
    public record CompanyGetDto(string name, string ceoName, int dateFounded, CompanySizeEnum companySize, string industry, string webLink, string description, string address);
}
