using CareerCompassAPI.Application.Abstraction.Repositories.IPaymentRepositories;
using CareerCompassAPI.Domain.Entities;
using CareerCompassAPI.Persistence.Contexts;

namespace CareerCompassAPI.Persistence.Implementations.Repositories.PaymentRepositories
{
    public class PaymentReadRepository : ReadRepository<Payments>, IPaymentReadRepository
    {
        public PaymentReadRepository(CareerCompassDbContext context) : base(context)
        {
        }
    }
}
