using AutoMapper;
using CareerCompassAPI.Application.Abstraction.Repositories.IPaymentRepositories;
using CareerCompassAPI.Application.Abstraction.Services;
using CareerCompassAPI.Application.DTOs.Dashboard_DTOs;
using CareerCompassAPI.Application.DTOs.Payment_DTOs;
using CareerCompassAPI.Domain.Concretes;
using CareerCompassAPI.Domain.Entities;
using CareerCompassAPI.Domain.Enums;
using CareerCompassAPI.Domain.Identity;
using CareerCompassAPI.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace CareerCompassAPI.Persistence.Implementations.Services
{
    public class PaymentService : IPaymentsService
    {
        private readonly IPaymentWriteRepository _paymentWriteRepository;
        private readonly IPaymentReadRepository _paymentReadRepository;
        private readonly IMapper _mapper;
        private readonly CareerCompassDbContext _context;
        public PaymentService(IPaymentWriteRepository paymentWriteRepository,
                              CareerCompassDbContext context,
                              IPaymentReadRepository paymentReadRepository,
                              IMapper mapper)
        {
            _paymentWriteRepository = paymentWriteRepository;
            _context = context;
            _paymentReadRepository = paymentReadRepository;
            _mapper = mapper;
        }

        public async Task CreateAsync(PaymentCreateDto paymentCreateDto)
        {
            if (paymentCreateDto is null)
            {
                throw new ArgumentNullException();
            }
            AppUser user = await _context.Users.FirstOrDefaultAsync(u => u.Id == paymentCreateDto.appUserId);
            Payments newPayment = new()
            {
                AppUser = user,
                Amount = paymentCreateDto.amount/100,
                Type = paymentCreateDto.type
            };
            await _paymentWriteRepository.AddAsync(newPayment);
            await _paymentWriteRepository.SaveChangesAsync();
        }

        public async Task<PaginatedResponse<PaymentsGetDto>> GetPaymentsByAppUserId(string appUserId, int currentPage = 1, int pageSize = 10)
        {
            if (appUserId is null)
            {
                throw new ArgumentNullException(nameof(appUserId), "AppUserId cannot be null");
            }

            AppUser user = await _context.Users.FirstOrDefaultAsync(u => u.Id == appUserId);
            var paymentsQuery = _context.Payments
                                         .Where(p => p.AppUser.Id == appUserId); 
            int totalCount = await paymentsQuery.CountAsync();
            var paginatedPayments = await paymentsQuery
                                        .Skip((currentPage - 1) * pageSize)
                                        .Take(pageSize)
                                        .ToListAsync();

            List<PaymentsGetDto> dtoList = paginatedPayments.Select(payment =>
                new PaymentsGetDto(
                    payment.Amount,
                    Enum.GetName(typeof(PaymentTypes), payment.Type),
                    payment.DateCreated)
                ).ToList();

            return new PaginatedResponse<PaymentsGetDto>(dtoList, totalCount);
        }

        public async Task<List<PaymentStatDto>> GetPaymentsStatsAsync(DateTime startDate, DateTime endDate)
        {
            var query = _context.Payments.AsQueryable();

            query = query.Where(p => p.DateCreated >= startDate && p.DateCreated <= endDate);

            var payments = await query.ToListAsync();

            return payments
                .GroupBy(p => p.DateCreated.Date)
                .Select(g => new PaymentStatDto(
                    Date: g.Key,
                    subscriptionCount: g.Count(p => p.Type == PaymentTypes.Subscription),
                    resumeCount: g.Count(p => p.Type == PaymentTypes.Resume)
                ))
                .OrderBy(x => x.Date)
                .ToList();
        }
    }
}
