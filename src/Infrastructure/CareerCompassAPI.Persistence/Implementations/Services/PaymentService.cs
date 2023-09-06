using AutoMapper;
using CareerCompassAPI.Application.Abstraction.Repositories.IPaymentRepositories;
using CareerCompassAPI.Application.Abstraction.Services;
using CareerCompassAPI.Application.DTOs.Payment_DTOs;
using CareerCompassAPI.Domain.Entities;
using CareerCompassAPI.Domain.Enums;
using CareerCompassAPI.Domain.Identity;
using CareerCompassAPI.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

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

        public async Task<List<PaymentsGetDto>> GetPaymentsByAppUserId(string appUserId)
        {
            if (appUserId is null)
            {
                throw new ArgumentNullException(nameof(appUserId), "AppUserId cannot be null");
            }

            AppUser user = await _context.Users.FirstOrDefaultAsync(u => u.Id == appUserId);
            var paymentsQuery = _paymentReadRepository.GetAllByExpression(
                                    p => p.AppUser == user,  
                                    take: 50,
                                    skip: 0
                                 );

            var payments = await paymentsQuery.ToListAsync();
            List<PaymentsGetDto> dtoList = payments.Select(payment =>
                    new PaymentsGetDto(
                        payment.Amount,
                        (PaymentTypes)payment.Type,
                        payment.DateCreated
                    )).ToList();
            return dtoList;
        }
    }
}
