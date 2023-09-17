using CareerCompassAPI.Application.Abstraction.Repositories.IFeedbackRepositories;
using CareerCompassAPI.Application.Abstraction.Services;
using CareerCompassAPI.Application.DTOs.Feedback_DTOs;
using CareerCompassAPI.Domain.Entities;
using CareerCompassAPI.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace CareerCompassAPI.Persistence.Implementations.Services
{
    public class FeedbackService : IFeedbackService
    {
        private readonly IFeedbackReadRepository _feedbackReadRepository;
        private readonly IFeedbackWriteRepository _feedbackWriteRepository;
        private readonly CareerCompassDbContext _context;

        public FeedbackService(IFeedbackWriteRepository feedbackWriteRepository,
                               IFeedbackReadRepository feedbackReadRepository,
                               CareerCompassDbContext context)
        {
            _feedbackWriteRepository = feedbackWriteRepository;
            _feedbackReadRepository = feedbackReadRepository;
            _context = context;
        }

        public async Task CreateFeedbackAsync(FeedbackCreateDto feedbackCreateDto)
        {
            if (feedbackCreateDto is null)
            {
                throw new ArgumentNullException("Parameters passed in may not contain null values");
            }
            TestimonialFeedback newFeedback = new()
            {
                FirstName = feedbackCreateDto.firstName,
                LastName = feedbackCreateDto.lastName,
                Description = feedbackCreateDto.description,
                ImageUrl = feedbackCreateDto.imageUrl,
                JobTitle = feedbackCreateDto.jobTitle,
                isActive = false
            };
            await _feedbackWriteRepository.AddAsync(newFeedback);
            await _feedbackWriteRepository.SaveChangesAsync();
        }
        public async Task<List<TestimonialFeedbackGetDto>> GetFeedbacksAsync()
        {
            var feedbacks = await _context.TestimonialFeedbacks
                                          .Where(f => f.isActive == true)
                                          .OrderByDescending(f => f.DateCreated)
                                          .Take(3)
                                          .ToListAsync();

            var feedbackDTOs = feedbacks.Select(f => new TestimonialFeedbackGetDto(
                f.FirstName,
                f.LastName,
                f.Description,
                f.ImageUrl,
                f.JobTitle
            )).ToList();

            return feedbackDTOs;
        }

    }
}