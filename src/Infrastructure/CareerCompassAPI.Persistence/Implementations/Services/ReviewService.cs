using CareerCompassAPI.Application.Abstraction.Repositories.ICompanyRepositories;
using CareerCompassAPI.Application.Abstraction.Repositories.IReviewRepositories;
using CareerCompassAPI.Application.Abstraction.Services;
using CareerCompassAPI.Application.DTOs.Review_DTOs;
using CareerCompassAPI.Domain.Entities;

namespace CareerCompassAPI.Persistence.Implementations.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewWriteRepository _reviewWriteRepository;
        private readonly IReviewReadRepository _reviewReadRepository;
        private readonly ICompanyReadRepository _companyReadRepository;

        public ReviewService(IReviewWriteRepository reviewWriteRepository,
                             IReviewReadRepository reviewReadRepository,
                             ICompanyReadRepository companyReadRepository)
        {
            _reviewWriteRepository = reviewWriteRepository;
            _reviewReadRepository = reviewReadRepository;
            _companyReadRepository = companyReadRepository;
        }

        public async Task CreateAsync(ReviewCreateDto reviewCreateDto)
        {
            if (reviewCreateDto is null)
            {
                throw new ArgumentNullException();
            }
            Company company = await _companyReadRepository.GetByIdAsync(reviewCreateDto.companyId);
            if (company is null)
            {
                throw new ArgumentNullException();
            }
            Review newReview = new()
            {
                Title = reviewCreateDto.title,
                Description = reviewCreateDto.description,
                Company = company
            };
            await _reviewWriteRepository.AddAsync(newReview);
            await _reviewWriteRepository.SaveChangesAsync();
        }
    }
}
