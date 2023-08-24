using CareerCompassAPI.Application.Abstraction.Repositories.IPaymentRepositories;
using CareerCompassAPI.Domain.Entities;
using CareerCompassAPI.Persistence.Contexts;

namespace CareerCompassAPI.Persistence.Implementations.Repositories.PaymentRepositories
{
    public class PaymentWriteRepository : WriteRepository<Payments>, IPaymentWriteRepository
    {
        public PaymentWriteRepository(CareerCompassDbContext context) : base(context)
        {
        }
    }
}
