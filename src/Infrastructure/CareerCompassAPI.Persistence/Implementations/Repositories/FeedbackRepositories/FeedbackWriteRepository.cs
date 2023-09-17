using CareerCompassAPI.Application.Abstraction.Repositories.IFeedbackRepositories;
using CareerCompassAPI.Domain.Entities;
using CareerCompassAPI.Persistence.Contexts;

namespace CareerCompassAPI.Persistence.Implementations.Repositories.FeedbackRepositories
{
    public class FeedbackWriteRepository : WriteRepository<TestimonialFeedback>, IFeedbackWriteRepository
    {
        public FeedbackWriteRepository(CareerCompassDbContext context) : base(context)
        {
        }
    }
}
