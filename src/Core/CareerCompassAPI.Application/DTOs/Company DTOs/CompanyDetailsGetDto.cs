using CareerCompassAPI.Domain.Entities;
using CareerCompassAPI.Domain.Enums;

namespace CareerCompassAPI.Application.DTOs.Company_DTOs
{
    public record CompanyDetailsGetDto(Guid companyId,string companyName, string description, string ceoName, int dateFounded, string companySize,string industryName, string webLink, string address, string logoUrl);
}
