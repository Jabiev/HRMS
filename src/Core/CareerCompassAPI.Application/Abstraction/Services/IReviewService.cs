using CareerCompassAPI.Application.DTOs.Review_DTOs;

namespace CareerCompassAPI.Application.Abstraction.Services
{
    public interface IReviewService
    {
        Task CreateAsync(ReviewCreateDto reviewCreateDto);
        Task<CompanyReviewSummaryDto> GetAllByCompanyId(Guid companyId);
    }
}
