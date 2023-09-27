namespace CareerCompassAPI.Application.DTOs.Company_DTOs
{
    public record CompanyDetailsUpdateDto(Guid id, string ceoName, string companyName, string webLink,string address, string description);
}
