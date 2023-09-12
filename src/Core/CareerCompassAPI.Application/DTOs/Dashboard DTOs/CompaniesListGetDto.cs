namespace CareerCompassAPI.Application.DTOs.Dashboard_DTOs
{
    public record CompaniesListGetDto(Guid companyId, string companyName, int followersCount, int reviewsCount, string location);
}
