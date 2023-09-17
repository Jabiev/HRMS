using CareerCompassAPI.Application.Abstraction.Repositories.IFeedbackRepositories;
using CareerCompassAPI.Application.Abstraction.Services;
using CareerCompassAPI.Application.DTOs.Feedback_DTOs;
using CareerCompassAPI.Domain.Entities;

namespace CareerCompassAPI.Persistence.Implementations.Services
{
    public class FeedbackService : IFeedbackService
    {
        private readonly IFeedbackWriteRepository _feedbackWriteRepository;

        public FeedbackService(IFeedbackWriteRepository feedbackWriteRepository)
        {
            _feedbackWriteRepository = feedbackWriteRepository;
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
            };
            await _feedbackWriteRepository.AddAsync(newFeedback);
            await _feedbackWriteRepository.SaveChangesAsync();
        }
    }
}
