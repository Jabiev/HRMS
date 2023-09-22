using CareerCompassAPI.Application.DTOs.Payment_DTOs;
using CareerCompassAPI.Domain.Concretes;

namespace CareerCompassAPI.Application.Abstraction.Services
{
    public interface IPaymentsService
    {
        Task CreateAsync(PaymentCreateDto paymentCreateDto);
        Task<PaginatedResponse<PaymentsGetDto>> GetPaymentsByAppUserId(string appUserId, int currentPage, int pageSize);
        Task<List<PaymentStatDto>> GetPaymentsStatsAsync(DateTime startDate, DateTime endDate);
    }
}
