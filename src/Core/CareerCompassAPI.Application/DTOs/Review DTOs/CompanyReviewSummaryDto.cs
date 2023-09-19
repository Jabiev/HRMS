namespace CareerCompassAPI.Application.DTOs.Review_DTOs
{
    public record CompanyReviewSummaryDto(List<ReviewGetDto> Reviews, decimal AverageRating, int TotalReviews);

}
