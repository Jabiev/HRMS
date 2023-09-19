using CareerCompassAPI.Application.Abstraction.Repositories.ICompanyRepositories;
using CareerCompassAPI.Application.Abstraction.Repositories.IJobSeekerRepositories;
using CareerCompassAPI.Application.Abstraction.Repositories.IReviewRepositories;
using CareerCompassAPI.Application.Abstraction.Services;
using CareerCompassAPI.Application.DTOs.Review_DTOs;
using CareerCompassAPI.Domain.Entities;
using CareerCompassAPI.Domain.Enums;
using CareerCompassAPI.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace CareerCompassAPI.Persistence.Implementations.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewWriteRepository _reviewWriteRepository;
        private readonly IReviewReadRepository _reviewReadRepository;
        private readonly ICompanyReadRepository _companyReadRepository;
        private readonly IJobSeekerReadRepository _jobSeekerReadRepository;
        private readonly CareerCompassDbContext _context;

        public ReviewService(IReviewWriteRepository reviewWriteRepository,
                             IReviewReadRepository reviewReadRepository,
                             ICompanyReadRepository companyReadRepository,
                             IJobSeekerReadRepository jobSeekerReadRepository,
                             CareerCompassDbContext context)
        {
            _reviewWriteRepository = reviewWriteRepository;
            _reviewReadRepository = reviewReadRepository;
            _companyReadRepository = companyReadRepository;
            _jobSeekerReadRepository = jobSeekerReadRepository;
            _context = context;
        }

        public async Task CreateAsync(ReviewCreateDto reviewCreateDto)
        {
            if (reviewCreateDto is null)
            {
                throw new ArgumentNullException(nameof(reviewCreateDto), "Review data should not be null.");
            }

            Company company = await _companyReadRepository.GetByIdAsync(reviewCreateDto.companyId);
            if (company is null)
            {
                throw new ArgumentNullException(nameof(company), "Company not found.");
            }

            JobSeeker js = await _context.JobSeekers.FirstOrDefaultAsync(j => j.AppUserId == reviewCreateDto.appUserId);
            if (js is null)
            {
                throw new ArgumentNullException(nameof(js), "JobSeeker not found.");
            }

            Review newReview = new()
            {
                JobSeeker = js,
                Title = reviewCreateDto.title,
                Description = reviewCreateDto.description,
                Rating = reviewCreateDto.rating,
                Status = Domain.Enums.ReviewStatus.Pending,
                Company = company
            };

            await _reviewWriteRepository.AddAsync(newReview);
            await _reviewWriteRepository.SaveChangesAsync();
        }

        public async Task<CompanyReviewSummaryDto> GetAllByCompanyId(Guid companyId, int pageIndex, int pageSize)
        {
            int skip = (pageIndex - 1) * pageSize;
            var reviews = await _context.Reviews
                                        .Where(r => r.Company.Id == companyId && r.Status == ReviewStatus.Approved)
                                        .Include(r => r.JobSeeker)
                                        .Skip(skip)
                                        .Take(pageSize)
                                        .ToListAsync();
            var reviewDtos = reviews.Select(r => new ReviewGetDto(
                r.JobSeeker.FirstName,
                r.JobSeeker.LastName,
                r.Title,
                r.Description,
                r.Rating)).ToList();

            decimal averageRating = 0;
            if (reviewDtos.Count > 0)
            {
                averageRating = reviewDtos.Average(r => r.rating);
            }
            int totalReviews = await _context.Reviews
                                              .Where(r => r.Company.Id == companyId && r.Status == ReviewStatus.Approved)
                                              .CountAsync();

            return new CompanyReviewSummaryDto(reviewDtos, averageRating, totalReviews);
        }
    }
}
