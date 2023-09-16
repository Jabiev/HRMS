namespace CareerCompassAPI.Application.DTOs.Company_DTOs
{
    public record HighestRatedCompanyGetDto(Guid companyId,string companyName, string logoUrl, int reviewsCount, decimal rating);
}
