using CareerCompassAPI.Application.Abstraction.Repositories.IReviewRepositories;
using CareerCompassAPI.Domain.Entities;
using CareerCompassAPI.Persistence.Contexts;

namespace CareerCompassAPI.Persistence.Implementations.Repositories.ReviewRepositories
{
    public class ReviewWriteRepository : WriteRepository<Review>, IReviewWriteRepository
    {
        public ReviewWriteRepository(CareerCompassDbContext context) : base(context)
        {
        }
    }
}
