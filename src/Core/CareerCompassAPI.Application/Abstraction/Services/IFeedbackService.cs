using CareerCompassAPI.Application.DTOs.Feedback_DTOs;

namespace CareerCompassAPI.Application.Abstraction.Services
{
    public interface IFeedbackService
    {
        Task CreateFeedbackAsync(FeedbackCreateDto feedbackCreateDto);
        Task<List<TestimonialFeedbackGetDto>> GetFeedbacksAsync();
    }
}
