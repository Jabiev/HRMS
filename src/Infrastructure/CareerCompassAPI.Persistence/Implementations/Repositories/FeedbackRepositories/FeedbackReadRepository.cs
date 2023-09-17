using CareerCompassAPI.Application.Abstraction.Repositories.IFeedbackRepositories;
using CareerCompassAPI.Domain.Entities;
using CareerCompassAPI.Persistence.Contexts;

namespace CareerCompassAPI.Persistence.Implementations.Repositories.FeedbackRepositories
{
    public class FeedbackReadRepository : ReadRepository<TestimonialFeedback>, IFeedbackReadRepository
    {
        public FeedbackReadRepository(CareerCompassDbContext context) : base(context)
        {
        }
    }
}
