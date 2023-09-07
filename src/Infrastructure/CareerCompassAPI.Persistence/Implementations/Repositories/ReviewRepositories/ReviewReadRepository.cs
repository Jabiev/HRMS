using CareerCompassAPI.Application.Abstraction.Repositories.IReviewRepositories;
using CareerCompassAPI.Persistence.Contexts;
using Review = CareerCompassAPI.Domain.Entities.Review;

namespace CareerCompassAPI.Persistence.Implementations.Repositories.ReviewRepositories
{
    public class ReviewReadRepository : ReadRepository<Review>, IReviewReadRepository
    {
        public ReviewReadRepository(CareerCompassDbContext context) : base(context)
        {
        }
    }
}
